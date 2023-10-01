using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Payables.Invoices;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceEditor
    {
        [Inject]
        public IVendorDataService? VendorDataService { get; set; }

        [Parameter]
        public VendorInvoiceToWrite? Invoice { get; set; }

        [Parameter]
        public long InvoiceId { get; set; }

        [Parameter]
        public EventCallback OnSaveAndExit { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [CascadingParameter]
        public DialogFactory? Dialogs { get; set; }

        [Inject]
        ILogger<VendorInvoiceEditor> Logger { get; set; }

        private Dictionary<string, object> ReportParameters = new Dictionary<string, object>();

        private InvoiceTotals InvoiceTotals { get; set; } = new();
        private string Title { get; set; } = "";
        private bool Printing = false;

        protected override void OnParametersSet()
        {
            CalculateTotals();

            Title = FormTitle.BuildTitle(FormMode, "Invoice");
        }

        private void CalculateTotals()
        {
            if (Invoice is not null)
            {
                InvoiceTotals.Calculate(Invoice);
                Invoice.Total = InvoiceTotals.Total;
                StateHasChanged();
            }
        }

        private string PostButtonText()
        {
            return "Complete";
        }

        private string UnpostButtonText()
        {
            return "Revert To Open";
        }

        private async Task OnCompleteAsync()
        {
            bool inBalance = InvoiceTotals.Total == InvoiceTotals.Payments;
            var result = (Invoice?.Vendor is not null)
                                ? await VendorDataService.GetAsync(Invoice.Vendor.Id)
                                : new();

            if (result.IsFailure)
            {
                Logger.LogError(result.Error);
                return;
            }

            var vendor = result.Value;

            // TODO: Check to see if invoice # has already been used.  Or is there a better way?
            // DONE (DE): implement invoice number uniqueness in VendorInvoice: unique for the selected vendor.
            // DONE (DE): Add a DocumentType field to the vendor invoice (Invoice/Return)
            //       Will need to support Statements too - another doctype or another entity?
            // DONE (DE): Vendors need a DefaultPaymentMethod field

            if (Invoice?.DocumentType == VendorInvoiceDocumentType.Invoice)
            {
                if (!inBalance && vendor?.DefaultPaymentMethod != null)
                {
                    if (Invoice.Payments?.Count == 1 && Invoice.Payments[0].PaymentMethod.Id == vendor.DefaultPaymentMethod.PaymentMethod.Id)
                    {
                        Invoice.Payments[0].Amount = InvoiceTotals.Total;
                        inBalance = true;
                    }
                    else if (Invoice.Payments?.Count == 0)
                    {
                        Invoice.Payments.Add(new()
                        {
                            PaymentMethod = vendor.DefaultPaymentMethod.PaymentMethod,
                            Amount = InvoiceTotals.Total
                        });
                        inBalance = true;
                    }
                    if (inBalance)
                    {
                        InvoiceTotals.Calculate(Invoice);
                        StateHasChanged();
                    }
                }

                if (!inBalance)
                {
                    await Dialogs.AlertAsync("The Invoice Total and Payment Total must match.", "Not Paid");
                    return;
                }
            }

            // TODO: If SimplyAccounting is in use then the Invoice # is required, along with sale codes on every purchase, stock return,
            //       core return, and defective & guaranteed replacement

            if (!await Dialogs.ConfirmAsync("Mark this invoice as Completed?", "Complete Invoice?"))
                return;

            CreateChargeLineItems();
            PostToInventory(true);
            RemoveCoreOnHands(true);

            Invoice.Status = VendorInvoiceStatus.Completed;
            Invoice.DatePosted = DateTime.Today;

            await OnSaveAndExit.InvokeAsync(Invoice);
        }

        private async Task OnRevertAsync()
        {
            bool saveIt = false;

            // TODO: Check to see if this invoice was put on a statement. Can't be unposted if it was.
            // FROM ENTERPRISE
            //if (tblVendDocHeader->StmntDocID.Length())
            //{
            //    UnicodeString msg = tblVendDocHeader->DocType == vdtINVOICE ? "Invoice" : "Return";
            //    msg = "This " + msg + " has been placed on a Statement\nand cannot be unposted.";
            //    Application->MessageBox(msg.c_str(), L"Posted Document", MB_OK + MB_ICONINFORMATION);
            //    return;
            //}

            if (Invoice.DocumentType == VendorInvoiceDocumentType.Invoice)
            {
                if (await Dialogs.ConfirmAsync("Revert this to an Open Invoice?", "Revert Invoice?"))
                {
                    ReverseInvoice();
                    saveIt = true;
                }

            }
            else if (Invoice.DocumentType == VendorInvoiceDocumentType.Return)
            {
                if (Invoice.Status == VendorInvoiceStatus.ReturnSent)
                {
                    if (await Dialogs.ConfirmAsync("Revert this to an Open Return?" + Environment.NewLine + "NOTE: This will add Stock Return items back into inventory.", "Revert Return?"))
                    {
                        if (PostReturn(false))
                        {
                            DeleteChargeLineItems();
                            RemoveCoreOnHands(false);
                            Invoice.Status = VendorInvoiceStatus.Open;
                            saveIt = true;
                        }
                    }
                }
                else if (Invoice.Status == VendorInvoiceStatus.Completed)
                {
                    if (await Dialogs.ConfirmAsync("Revert this to a Sent Return?", "Revert Return?"))
                    {
                        DeleteChargeLineItems();
                        Invoice.Status = VendorInvoiceStatus.ReturnSent;
                        Invoice.DatePosted = null;
                        saveIt = true;
                    }
                }
            }

            if (saveIt)
            {
                await OnSave.InvokeAsync(Invoice);
                FormMode = FormMode.Edit;
                StateHasChanged();
            }
        }

        private void OnPrint()
        {
            // TODO: Need to get the real shop name and number to display on the report
            var documentTypeString = Invoice.DocumentType == VendorInvoiceDocumentType.Invoice ? "Vendor Invoice" : Invoice.DocumentType.GetDisplayName();
            var nullDate = new DateTime(1899, 1, 1);
            ReportParameters.Clear();
            ReportParameters.Add("VendorInvoiceId", InvoiceId);
            ReportParameters.Add("ShopName", "Al's AutoCare (need real name & shop #)");
            ReportParameters.Add("ShopNumber", 1);
            ReportParameters.Add("DocumentTypeString", documentTypeString);
            ReportParameters.Add("VendorCode", Invoice.Vendor.VendorCode);
            ReportParameters.Add("VendorName", Invoice.Vendor.Name);
            ReportParameters.Add("VendorStreet", Invoice.Vendor.Address.AddressLine1);
            ReportParameters.Add("VendorCityStateZip", $"{Invoice.Vendor.Address.City}, {Invoice.Vendor.Address.State} {Invoice.Vendor.Address.PostalCode}");
            ReportParameters.Add("InvoiceNumber", Invoice.InvoiceNumber);
            ReportParameters.Add("DateCreated", Invoice.Date != null ? Invoice.Date : nullDate);
            ReportParameters.Add("DatePosted", Invoice.DatePosted != null ? Invoice.DatePosted : nullDate);
            ReportParameters.Add("StatusString", Invoice.Status.GetDisplayName());
            ReportParameters.Add("Total", InvoiceTotals.Total);
            ReportParameters.Add("Balance", InvoiceTotals.Total - InvoiceTotals.Payments);
            Printing = true;
        }

        private void HideReport()
        {
            Printing = false;
        }

        private void ReverseInvoice()
        {
            // TEMPORARY...
            Invoice.Status = VendorInvoiceStatus.Open;
            Invoice.DatePosted = null;

            // FROM ENTERPRISE...
            //UnicodeString revDocID, newDocID, oldDocID = tblVendDocHeader->VendDocID;
            //TStringList* lstOldItemIDs = new TStringList;

            //tblVendDocHeaderCopy->Active = true;
            //tblVendDocDetailCopy->Active = true;
            //tblVendDocTaxCopy->Active = true;
            //tblVendDocPayCopy->Active = true;

            DeleteChargeLineItems();
            RemoveCoreOnHands(removing: false);
            PostToInventory(posting: false);

            // FROM ENTERPRISE...
            //// Create the negative invoice header
            //tblVendDocHeader->CopyRec(tblVendDocHeaderCopy);
            //tblVendDocHeaderCopy->Edit();
            //tblVendDocHeaderCopy->DocTotal = -tblVendDocHeaderCopy->DocTotal;
            //tblVendDocHeaderCopy->TaxTotal = -tblVendDocHeaderCopy->TaxTotal;
            //tblVendDocHeaderCopy->PostDate = AcctDate;
            //tblVendDocHeaderCopy->ReversalOfID = oldDocID;
            //tblVendDocHeaderCopy->Post();
            //revDocID = tblVendDocHeaderCopy->VendDocID;

            //// Create the negative items
            //for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //{
            //    lstOldItemIDs->Add(tblVendDocDetail->ID);
            //    tblVendDocDetail->CopyRec(tblVendDocDetailCopy);
            //    tblVendDocDetailCopy->Edit();
            //    tblVendDocDetailCopy->VendDocID = revDocID;
            //    tblVendDocDetailCopy->Qty = -tblVendDocDetailCopy->Qty;
            //    tblVendDocDetailCopy->Post();
            //}

            //// Create the negative taxes
            //for (tblVendDocTax->First(); !tblVendDocTax->Eof; tblVendDocTax->Next())
            //{
            //    tblVendDocTax->CopyRec(tblVendDocTaxCopy);
            //    tblVendDocTaxCopy->Edit();
            //    tblVendDocTaxCopy->DocID = revDocID;
            //    tblVendDocTaxCopy->Amount = -tblVendDocTaxCopy->Amount;
            //    tblVendDocTaxCopy->Post();
            //}

            //// Create the negative payments
            //for (tblVendDocPay->First(); !tblVendDocPay->Eof; tblVendDocPay->Next())
            //{
            //    tblVendDocPay->CopyRec(tblVendDocPayCopy);
            //    tblVendDocPayCopy->Edit();
            //    tblVendDocPayCopy->DocID = revDocID;
            //    tblVendDocPayCopy->Amount = -tblVendDocPayCopy->Amount;
            //    tblVendDocPayCopy->Post();
            //}

            //// Mark original invoice as reversed by this invoice
            //tblVendDocHeader->Edit();
            //tblVendDocHeader->ReversedByID = revDocID;
            //tblVendDocHeader->Post();

            //// Create the positive, unposted invoice header
            //tblVendDocHeader->CopyRec(tblVendDocHeaderCopy);
            //tblVendDocHeaderCopy->Edit();
            //tblVendDocHeaderCopy->Status = vdsBUILDING;
            //tblVendDocHeaderCopy->PostDate = TDateTime(0);
            //tblVendDocHeaderCopy->Post();
            //newDocID = tblVendDocHeaderCopy->VendDocID;

            //// Create the positive items
            //int idx = 0;
            //for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //{
            //    tblVendDocDetail->CopyRec(tblVendDocDetailCopy);
            //    tblVendDocDetailCopy->Edit();
            //    tblVendDocDetailCopy->VendDocID = newDocID;
            //    tblVendDocDetailCopy->Post();

            //    // Point the item on customer invoice to this new copy
            //    if (lstOldItemIDs->Strings[idx].Length() && dtmSuppliers->tblDocItem->GetVendorItemID(lstOldItemIDs->Strings[idx]))
            //    {
            //        dtmSuppliers->tblDocItem->Edit();
            //        dtmSuppliers->tblDocItem->VendorItemID = tblVendDocDetailCopy->ID;
            //        dtmSuppliers->tblDocItem->Post();
            //    }
            //    idx++;
            //}

            //// Create the positive taxes
            //for (tblVendDocTax->First(); !tblVendDocTax->Eof; tblVendDocTax->Next())
            //{
            //    tblVendDocTax->CopyRec(tblVendDocTaxCopy);
            //    tblVendDocTaxCopy->Edit();
            //    tblVendDocTaxCopy->DocID = newDocID;
            //    tblVendDocTaxCopy->Post();
            //}

            //// Create the positive payments
            //for (tblVendDocPay->First(); !tblVendDocPay->Eof; tblVendDocPay->Next())
            //{
            //    tblVendDocPay->CopyRec(tblVendDocPayCopy);
            //    tblVendDocPayCopy->Edit();
            //    tblVendDocPayCopy->DocID = newDocID;
            //    tblVendDocPayCopy->Post();
            //}

            //// Point to new invoice & set status to building
            //VendDocID = newDocID;
            //cbxVendor->OnChange = NULL;
            //tblVendDocHeader->FindVendDoc(newDocID);
            //tblVendDocHeader->Edit();
            //tblVendDocHeader->Status = vdsBUILDING;
            //tblVendDocHeader->PostDate = TDateTime(0);
            //tblVendDocHeader->Post();
            //cbxVendor->OnChange = cbxVendorChange;
            //tblVendDocDetail->SetVendDocRange(newDocID);

            //tblVendDocHeaderCopy->Active = false;
            //tblVendDocDetailCopy->Active = false;
            //tblVendDocTaxCopy->Active = false;
            //tblVendDocPayCopy->Active = false;

            //delete lstOldItemIDs;
        }

        private void CreateChargeLineItems()
        {
            foreach (var payment in Invoice.Payments)
            {
                //if (payment.PaymentMethod.IsOnAccountPaymentType)
                if (payment.PaymentMethod.PaymentType == VendorInvoicePaymentMethodType.Charge)
                {
                    // TODO: Charge payment types result in unassigned balance forward type line items.
                    // FROM ENTERPRISE...
                    //tblVendDocDetailAvail->Insert();
                    //tblVendDocDetailAvail->SuppNum = tblVendDocHeader->SuppNum;
                    //tblVendDocDetailAvail->Type = apBALFORWARD;
                    //tblVendDocDetailAvail->Desc = "Bal. Fwd. from Inv #" + tblVendDocHeader->DocNum;
                    //tblVendDocDetailAvail->Qty = 1.0;
                    //tblVendDocDetailAvail->SuppCost = tblVendDocPay->Amount;
                    //tblVendDocDetailAvail->TransDate = Now();
                    //tblVendDocDetailAvail->ChargeDocID = tblVendDocHeader->VendDocID;
                    //tblVendDocDetailAvail->Post();
                }
            }
        }

        private void DeleteChargeLineItems()
        {
            // If the payment method(s) on this invoice was charge, it resulted in an unassigned balance forward type line item.
            // It would then be available to be added to a subsequent invoice or statement.  We need to delete the charge
            // line item(s) created from this invoice.

            // FROM ENTERPRISE...
            //bool wasActive = tblVendDocDetailAvail->Active;
            //if (!wasActive)
            //    tblVendDocDetailAvail->Active = true;

            //tblVendDocDetailAvail->SetVendChargeDocRange(tblVendDocHeader->SuppNum, tblVendDocHeader->VendDocID);
            //tblVendDocDetailAvail->First();
            //while (!tblVendDocDetailAvail->Eof)
            //{
            //    tblVendDocDetailAvail->Delete();
            //    tblVendDocDetailAvail->First();
            //}
            //tblVendDocDetailAvail->CancelRange();

            //if (!wasActive)
            //    tblVendDocDetailAvail->Active = false;
        }

        private void RemoveCoreOnHands(bool removing)
        {
            // FROM ENTERPRISE...
            //dtmSuppliers->tblShopInv->Active = true;
            //for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //{
            //    if (tblVendDocDetail->Type == apCORE
            //    && tblVendDocDetail->Core != 0.0
            //    && tblVendDocDetail->Mfg.Length()
            //    && tblVendDocDetail->Mfg != MFG_CUSTOM
            //    && dtmSuppliers->tblShopInv->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //    {
            //        dtmSuppliers->tblShopInv->Edit();
            //        if (removing)
            //            dtmSuppliers->tblShopInv->UsedCores = RoundDbl(dtmSuppliers->tblShopInv->UsedCores - tblVendDocDetail->Qty);
            //        else
            //            dtmSuppliers->tblShopInv->UsedCores = RoundDbl(dtmSuppliers->tblShopInv->UsedCores + tblVendDocDetail->Qty);
            //        dtmSuppliers->tblShopInv->Post();
            //    }
            //}
            //tblVendDocDetail->First();
            //dtmSuppliers->tblShopInv->Active = false;
        }

        private void PostToInventory(bool posting)
        {
            // FROM ENTERPRISE...
            //double qty;

            //dtmSuppliers->tblMaster->Active = true;
            //dtmSuppliers->tblShopInv->Active = true;
            //dtmSuppliers->tblPartHist->Active = true;
            //dtmSuppliers->tblSuppMfg->Active = true;
            //for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //{
            //    // Searching for part number using blank mfg could find wrong part -- make it a custom
            //    if (!tblVendDocDetail->Mfg.Length())
            //    {
            //        tblVendDocDetail->Edit();
            //        if (!tblVendDocDetail->PostToInventory || !CustomStockedInUse)
            //            tblVendDocDetail->Mfg = MFG_CUSTOM;
            //        else
            //            tblVendDocDetail->Mfg = MFG_CUSTOM_STOCKED;
            //        tblVendDocDetail->Post();
            //    }

            //    if (tblVendDocDetail->ItemNum == -1 && tblVendDocDetail->PostToInventory
            //    && (tblVendDocDetail->Type == apPURCHASE || tblVendDocDetail->Type == apRETURN
            //        || tblVendDocDetail->Type == apDEFECT || tblVendDocDetail->Type == apGUARANTEE))
            //    {
            //        if (CustomStockedInUse && tblVendDocDetail->Mfg == MFG_CUSTOM)
            //        {
            //            tblVendDocDetail->Edit();
            //            tblVendDocDetail->Mfg = MFG_CUSTOM_STOCKED;
            //            tblVendDocDetail->Post();
            //        }

            //        bool found = dtmSuppliers->tblMaster->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum);
            //        if (CustomStockedInUse && tblVendDocDetail->Mfg == MFG_CUSTOM_STOCKED)
            //        {
            //            if (!found)
            //            {
            //                dtmSuppliers->tblMaster->Insert();
            //                dtmSuppliers->tblMaster->Mfg = tblVendDocDetail->Mfg;
            //                dtmSuppliers->tblMaster->FullStock = tblVendDocDetail->PartNum;
            //                found = true;
            //            }
            //            else
            //                dtmSuppliers->tblMaster->Edit();

            //            // Overwrite these values each time since it may not really be the same part
            //            dtmSuppliers->tblMaster->Descr = tblVendDocDetail->Desc;
            //            dtmSuppliers->tblMaster->ProdCode = tblVendDocDetail->SaleCode + "00";
            //            dtmSuppliers->tblMaster->PopCode = 'A';
            //            dtmSuppliers->tblMaster->PartType = ptREGULAR;
            //            dtmSuppliers->tblMaster->PkgQty = 1;
            //            dtmSuppliers->tblMaster->Selling[0] = 0.0;
            //            dtmSuppliers->tblMaster->Selling[1] = 0.0;
            //            dtmSuppliers->tblMaster->Selling[2] = 0.0;
            //            dtmSuppliers->tblMaster->Selling[3] = 0.0;
            //            dtmSuppliers->tblMaster->Selling[4] = 0.0;
            //            dtmSuppliers->tblMaster->DfltSell = 1;
            //            dtmSuppliers->tblMaster->List = 0.0;
            //            dtmSuppliers->tblMaster->Cost = tblVendDocDetail->SuppCost;
            //            dtmSuppliers->tblMaster->AvgCost = tblVendDocDetail->SuppCost;
            //            dtmSuppliers->tblMaster->Core = tblVendDocDetail->Core;
            //            dtmSuppliers->tblMaster->Post();
            //        }
            //        if (found)
            //        {
            //            if (!dtmSuppliers->tblShopInv->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //                dtmSuppliers->tblShopInv->ActivatePart(dtmSuppliers->tblMaster, config.DefOrderCode);
            //            dtmSuppliers->tblShopInv->Edit();
            //            if (tblVendDocDetail->Type == apPURCHASE)
            //            {
            //                qty = (posting) ? tblVendDocDetail->Qty : -tblVendDocDetail->Qty;
            //                if (dtmSuppliers->tblMaster->Cost != tblVendDocDetail->SuppCost)
            //                {
            //                    dtmSuppliers->tblSuppMfg->Index = ndxAPMFG_SUPPMFG;
            //                    dtmSuppliers->tblSuppMfg->SetRangeStart();
            //                    dtmSuppliers->tblSuppMfg->SupplierNum = tblVendDocDetail->SuppNum;
            //                    dtmSuppliers->tblSuppMfg->MFG = tblVendDocDetail->Mfg;
            //                    dtmSuppliers->tblSuppMfg->KeyFieldCount = 2;
            //                    dtmSuppliers->tblSuppMfg->SetRangeEnd();
            //                    dtmSuppliers->tblSuppMfg->SupplierNum = tblVendDocDetail->SuppNum;
            //                    dtmSuppliers->tblSuppMfg->MFG = tblVendDocDetail->Mfg;
            //                    dtmSuppliers->tblSuppMfg->KeyFieldCount = 2;
            //                    dtmSuppliers->tblSuppMfg->ApplyRange();
            //                    if (dtmSuppliers->tblSuppMfg->RecordCount && dtmSuppliers->tblSuppMfg->Preferred)
            //                    {
            //                        dtmSuppliers->tblMaster->Edit();
            //                        dtmSuppliers->tblMaster->Cost = tblVendDocDetail->SuppCost;
            //                        dtmSuppliers->tblMaster->Post();
            //                    }
            //                }
            //            }
            //            else if (tblVendDocDetail->Type == apRETURN || tblVendDocDetail->Type == apDEFECT || tblVendDocDetail->Type == apGUARANTEE)
            //                qty = (posting) ? -tblVendDocDetail->Qty : tblVendDocDetail->Qty;
            //            dtmSuppliers->tblShopInv->OnHand = RoundDbl(dtmSuppliers->tblShopInv->OnHand + qty);
            //            dtmSuppliers->tblShopInv->Post();

            //            dtmSuppliers->tblPartHist->Append();
            //            dtmSuppliers->tblPartHist->Mfg = dtmSuppliers->tblShopInv->Mfg;
            //            dtmSuppliers->tblPartHist->FullStock = dtmSuppliers->tblShopInv->FullStock;
            //            dtmSuppliers->tblPartHist->Date = Now();
            //            if (tblVendDocDetail->Type == apPURCHASE)
            //                dtmSuppliers->tblPartHist->DocType = hdtORDER;
            //            else if (tblVendDocDetail->Type == apRETURN || tblVendDocDetail->Type == apDEFECT || tblVendDocDetail->Type == apGUARANTEE)
            //                dtmSuppliers->tblPartHist->DocType = hdtRETURN;
            //            dtmSuppliers->tblPartHist->Outside = false;
            //            dtmSuppliers->tblPartHist->DocGUID = tblVendDocHeader->VendDocID;
            //            dtmSuppliers->tblPartHist->Qty = qty;
            //            dtmSuppliers->tblPartHist->SupplierID = tblVendDocHeader->SuppNum;
            //            dtmSuppliers->tblPartHist->Cost = tblVendDocDetail->SuppCost;
            //            dtmSuppliers->tblPartHist->Post();
            //        }
            //    }
            //}
            //tblVendDocDetail->First();
            //dtmSuppliers->tblMaster->Active = false;
            //dtmSuppliers->tblShopInv->Active = false;
            //dtmSuppliers->tblPartHist->Active = false;
            //dtmSuppliers->tblSuppMfg->Active = false;
        }

        private bool PostReturn(bool posting)
        {
            bool result = true;
            // FROM ENTERPRISE...
            //TEDBStringsArray fileList;

            //dtmSuppliers->tblMaster->Active = true;
            //dtmSuppliers->tblShopInv->Active = true;
            //dtmSuppliers->tblPartHist->Active = true;
            //try
            //{
            //    //warn about trying post more than onhand
            //    if (Post)
            //    {
            //        TStringList* msg = new TStringList();
            //        double onhand, committed;
            //        for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //        {
            //            if (tblVendDocDetail->Type == apRETURN
            //            && tblVendDocDetail->Mfg.Length()
            //            && tblVendDocDetail->Mfg != MFG_CUSTOM
            //            && dtmSuppliers->tblMaster->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //            {
            //                if (dtmSuppliers->tblShopInv->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //                {
            //                    onhand = dtmSuppliers->tblShopInv->OnHand;
            //                    committed = dtmSuppliers->tblShopInv->Committed;
            //                }
            //                else
            //                {
            //                    onhand = 0.0;
            //                    committed = 0.0;
            //                }

            //                if ((onhand - committed) < tblVendDocDetail->Qty)
            //                    msg->Add("Part " + tblVendDocDetail->PartNum + ": Returning " + AnsiString(tblVendDocDetail->Qty) + ", OnHand " + AnsiString(onhand) + ", Committed " + AnsiString(committed));
            //            }
            //        }

            //        if (msg->Count)
            //        {
            //            if (Application->MessageBox(UnicodeString("You are trying to stock return more than what is available of the following parts. Continue Posting?\r\n" + msg->Text).c_str(), L"Post Credit Return", MB_YESNO + MB_ICONQUESTION) != IDYES)
            //                result = false;
            //        }
            //        delete msg;
            //    }

            //    //update inventory onhand and history
            //    if (result)
            //    {
            //        fileList.Length = 2;
            //        fileList[0] = dtmSuppliers->tblShopInv->TableName;
            //        fileList[1] = dtmSuppliers->tblPartHist->TableName;
            //        dtmEnterprise->dbsShop->StartTransaction(fileList);

            //        for (tblVendDocDetail->First(); !tblVendDocDetail->Eof; tblVendDocDetail->Next())
            //        {
            //            if (tblVendDocDetail->Type == apRETURN
            //            && tblVendDocDetail->Mfg.Length()
            //            && tblVendDocDetail->Mfg != MFG_CUSTOM
            //            && dtmSuppliers->tblMaster->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //            {
            //                if (!dtmSuppliers->tblShopInv->FindPart(tblVendDocDetail->Mfg, tblVendDocDetail->PartNum))
            //                    dtmSuppliers->tblShopInv->ActivatePart(dtmSuppliers->tblMaster, config.DefOrderCode);

            //                dtmSuppliers->tblShopInv->Edit();
            //                if (Post)
            //                    dtmSuppliers->tblShopInv->OnHand = RoundDbl(dtmSuppliers->tblShopInv->OnHand - tblVendDocDetail->Qty);
            //                else
            //                    dtmSuppliers->tblShopInv->OnHand = RoundDbl(dtmSuppliers->tblShopInv->OnHand + tblVendDocDetail->Qty);
            //                dtmSuppliers->tblShopInv->Post();

            //                dtmSuppliers->tblPartHist->Append();
            //                dtmSuppliers->tblPartHist->Mfg = dtmSuppliers->tblShopInv->Mfg;
            //                dtmSuppliers->tblPartHist->FullStock = dtmSuppliers->tblShopInv->FullStock;
            //                dtmSuppliers->tblPartHist->Date = Now();
            //                dtmSuppliers->tblPartHist->DocType = hdtRETURN;
            //                dtmSuppliers->tblPartHist->Outside = false;
            //                dtmSuppliers->tblPartHist->DocGUID = tblVendDocHeader->VendDocID;
            //                if (Post)
            //                    dtmSuppliers->tblPartHist->Qty = -tblVendDocDetail->Qty;
            //                else
            //                    dtmSuppliers->tblPartHist->Qty = tblVendDocDetail->Qty;
            //                dtmSuppliers->tblPartHist->SupplierID = tblVendDocHeader->SuppNum;
            //                dtmSuppliers->tblPartHist->Cost = tblVendDocDetail->SuppCost;
            //                dtmSuppliers->tblPartHist->Post();
            //            }
            //        }

            //        dtmEnterprise->dbsShop->Commit();
            //    }
            //}
            //catch (Exception &E)
            //      {
            //    if (dtmEnterprise->dbsShop->InTransaction)
            //        dtmEnterprise->dbsShop->Rollback();
            //    result = false;
            //    ErrMsg(ETYPE_ERROR, 0, E.Message, "", MOD_NAME);
            //}

            //dtmSuppliers->tblMaster->Active = false;
            //dtmSuppliers->tblShopInv->Active = false;
            //dtmSuppliers->tblPartHist->Active = false;
            return result;
        }
    }
}
