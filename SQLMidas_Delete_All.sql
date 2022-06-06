--SELECT '_StagePersonCustomer';

--DELETE
--  FROM [dbo]._StagePersonCustomer
--  WHERE Id > '';
  
--DELETE
--  FROM [dbo].InventoryItemStage
--  WHERE Id > '';

DELETE
  FROM [dbo].InventoryItem
  WHERE Id > '';

DELETE
  FROM [dbo].ProductCode
  WHERE Code > '';

DELETE
  FROM [dbo].ProductCodeStage
  WHERE Code > '';

DELETE
  FROM [dbo].Manufacturer
  WHERE Code > '';

DELETE
  FROM [dbo].SaleCode
  WHERE Code > '';

DELETE
  FROM [dbo].CreditCard
  WHERE Name > '';

DELETE
  FROM [dbo].SalesTax
  WHERE Id > '';
