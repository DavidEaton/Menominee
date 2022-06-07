USE [midas]
GO

-- Load Manufacturer --------------------------------------------------

DROP VIEW Load_v
GO

CREATE view Load_v
AS
SELECT
Id,
Code,
Prefix,
Name

FROM Manufacturer
GO

BULK INSERT Manufacturer

FROM 'C:\stwin3\Server\INVENMfgs.txt'
WITH
(FIRSTROW= 2,
FIELDTERMINATOR='\t',
ROWTERMINATOR='\r'
);

-- Load SaleCode --------------------------------------------------
DROP VIEW Load_v
GO

CREATE view Load_v
AS
SELECT
Id,
Code,
Name,
DesiredMargin,
LaborRate,
IncludeLabor,
IncludeParts,
MaximumCharge,
MinimumCharge,
MinimumJobAmount,
Percentage

FROM SaleCode
GO

BULK INSERT SaleCode

FROM 'C:\stwin3\Server\SALESSaleCodes.txt'
WITH
(FIRSTROW= 2,
FIELDTERMINATOR='\t',
ROWTERMINATOR='\r'
);

-- Load ProductCode --------------------------------------------------
DROP VIEW Load_v
GO

CREATE view Load_v
AS
SELECT
Id,
Manufacturer,
ManufacturerId,
Code,
SaleCode,
SaleCodeId,
Name

FROM ProductCodeStage
GO

BULK INSERT ProductCodeStage

FROM 'C:\stwin3\Server\INVENProdCode.txt'
WITH
(FIRSTROW= 2,
FIELDTERMINATOR='\t',
ROWTERMINATOR='\r'
);


UPDATE dbo.ProductCodeStage
SET ManufacturerId = (SELECT Id 
FROM dbo.Manufacturer 
WHERE Manufacturer = Code)

UPDATE [dbo].[ProductCodeStage]
SET SaleCodeId = (SELECT Id 
FROM dbo.SaleCode
WHERE SaleCode = Code)

SET IDENTITY_INSERT ProductCode ON;

INSERT INTO [dbo].[ProductCode] (Id, ManufacturerId, Code, SaleCodeId, Name)
SELECT Id, ManufacturerId, Code, SaleCodeId, Name
FROM [dbo].[ProductCodeStage]

SET IDENTITY_INSERT ProductCode OFF;

-- Load CreditCard --------------------------------------------------
DROP VIEW Load_v
GO

CREATE view Load_v
AS
SELECT
Id,
Name,
FeeType,
Fee,
IsAddedToDeposit

FROM CreditCard
GO

BULK INSERT CreditCard

FROM 'C:\stwin3\Server\SALESCreditCard.txt'
WITH
(FIRSTROW= 2,
FIELDTERMINATOR='\t',
ROWTERMINATOR='\r'
);

-- Load SalesTax --------------------------------------------------
DROP VIEW Load_v
GO

CREATE view Load_v
AS
SELECT
Id,
[Order],
Description,
TaxType,
TaxIdNumber,
PartTaxRate,
LaborTaxRate,
IsAppliedByDefault,
IsTaxable

FROM SalesTax
GO

BULK INSERT SalesTax

FROM 'C:\stwin3\Server\SALESTaxRates.txt'
WITH
(FIRSTROW= 2,
FIELDTERMINATOR='\t',
ROWTERMINATOR='\r'
);


