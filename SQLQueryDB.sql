CREATE DATABASE FoodOrderSystemDB;
GO
USE FoodOrderSystemDB;
GO
CREATE TABLE [dbo].[role](
	[id] INT IDENTITY(1,1) NOT NULL,
	[role_name] varchar(25),
	CONSTRAINT [pk_role] PRIMARY KEY ([id]),
);
GO
CREATE TABLE [dbo].[user](
	[id] INT IDENTITY(1,1) NOT NULL,
	[role_id] INT,
	[name] nvarchar(30) NOT NULL,
	[surname] nvarchar(30) NOT NULL,
	[email] nvarchar(255) UNIQUE NOT NULL,
	[phone] varchar(25) UNIQUE NOT NULL,
	[password] nvarchar(255) NOT NULL,
	CONSTRAINT [pk_user] PRIMARY KEY ([id]),
	CONSTRAINT [fk_user_role] FOREIGN KEY ([role_id]) 
	REFERENCES [role] ([id]) 
	ON DELETE SET NULL 
	ON UPDATE CASCADE
);
GO
CREATE TABLE [dbo].[card](
	[id] INT IDENTITY(1,1) NOT NULL,
	[user_id] INT,
	[card_name] NVARCHAR(20),
	[card_number] NVARCHAR(16),
	[expiration_date] date,
	CONSTRAINT [pk_card] PRIMARY KEY ([id]),
	CONSTRAINT [fk_card_user] FOREIGN KEY ([user_id])
	REFERENCES [user] ([id])
	ON DELETE SET NULL
	ON UPDATE CASCADE
);
GO
CREATE TABLE [dbo].[province](
	[id] INT IDENTITY(1,1) NOT NULL,
	[province_name] NVARCHAR(30) NOT NULL,
	CONSTRAINT [pk_province] PRIMARY KEY ([id]),
);
GO
CREATE TABLE [dbo].[district](
	[id] INT IDENTITY(1,1) NOT NULL,
	[district_name] NVARCHAR(30) NOT NULL,
	CONSTRAINT [pk_district] PRIMARY KEY ([id]),
);
GO
CREATE TABLE [dbo].[address](
	[id] INT IDENTITY(1,1) NOT NULL,
	[province_id] INT,
	[district_id] INT,
	[neighbourhood] NVARCHAR(30),
	[street] NVARCHAR(30),
	[apartment_name] NVARCHAR(20),
	[postal_code] NVARCHAR(5) NOT NULL,
	[apartment_number] TINYINT,
	[floor] NVARCHAR(15),
	[description] TEXT,
	[tag] NVARCHAR(15),
	CONSTRAINT [pk_address] PRIMARY KEY ([id]),
	CONSTRAINT [fk_address_province] FOREIGN KEY ([province_id])
	REFERENCES [province] ([id])
	ON DELETE SET NULL
	ON UPDATE CASCADE,
	CONSTRAINT [fk_address_district] FOREIGN KEY ([district_id])
	REFERENCES [district] ([id])
	ON DELETE SET NULL
	ON UPDATE CASCADE
);
GO
CREATE TABLE [dbo].[address_user](
	[adres_id] INT,
	[user_id] INT,
	CONSTRAINT [pk_address_user] PRIMARY KEY ([adres_id],[user_id]),
	CONSTRAINT [fk_addressUser_address] FOREIGN KEY ([adres_id])
	REFERENCES [address] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [fk_addressUser_user] FOREIGN KEY ([user_id])
	REFERENCES [user] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[company](
	[id] INT IDENTITY(1,1) NOT NULL,
	[role_id] INT,
	[company_name] NVARCHAR(50),
	[LogoPath]	NVARCHAR (MAX) NOT NULL,
	[address_id] INT,
	[email] nvarchar(255) UNIQUE NOT NULL,
	[phone] varchar(25) UNIQUE NOT NULL,
	[password] nvarchar(255) NOT NULL,
	CONSTRAINT [pk_company] PRIMARY KEY ([id]),
	CONSTRAINT [fk_company_address] FOREIGN KEY ([address_id])
	REFERENCES [address] ([id])
	ON DELETE SET NULL
	ON UPDATE CASCADE,
	CONSTRAINT [fk_company_role] FOREIGN KEY ([role_id])
	REFERENCES [role] ([id]) 
	ON DELETE SET NULL
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[category](
	[id] INT IDENTITY(1,1) NOT NULL,
	[company_id] INT,
	[category_name] NVARCHAR(20),
	CONSTRAINT [pk_category] PRIMARY KEY ([id]),
	CONSTRAINT [fk_category_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[product](
	[id] INT IDENTITY(1,1) NOT NULL,
	[category_id] INT,
	[company_id] INT,
	[product_name] NVARCHAR(30) NOT NULL,
	[unit_price] smallmoney NOT NULL,
	[content] NVARCHAR(255),
	[ProductImagePath] NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
	CONSTRAINT [pk_product] PRIMARY KEY ([id]),
	CONSTRAINT [fk_product_category] FOREIGN KEY ([category_id])
	REFERENCES [category] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [fk_product_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
);
GO
CREATE TABLE [dbo].[comment](
	[id] INT IDENTITY(1,1) NOT NULL,
	[user_id] INT,
	[company_id] INT,
	[comment] text,
	[date] datetime2,
	CONSTRAINT [pk_comment] PRIMARY KEY ([id]),
	CONSTRAINT [fk_comment_user] FOREIGN KEY ([user_id])
	REFERENCES [user] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [fk_comment_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [payment_type](
	[id] INT IDENTITY(1,1) NOT NULL,
	[payment_name] nvarchar(20),
	CONSTRAINT [pk_paymentType] PRIMARY KEY ([id]),
);
GO
CREATE TABLE [dbo].[order](
	[id] INT  IDENTITY(1,1) NOT NULL,
	[user_id] INT NOT NULL,
	[company_id] INT NOT NULL,
	[payment_type] INT NOT NULL,
	[address_id] INT NOT NULL,
	[total_price] smallmoney NOT NULL,
	[created_at] timestamp,
	CONSTRAINT [pk_order] PRIMARY KEY ([id]),
	CONSTRAINT [fk_order_user] FOREIGN KEY ([user_id])
	REFERENCES [user] ([id])
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
	CONSTRAINT [fk_order_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
	CONSTRAINT [fk_order_paymentType] FOREIGN KEY ([payment_type])
	REFERENCES [payment_type] ([id])
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
	CONSTRAINT [fk_order_address] FOREIGN KEY ([address_id])
	REFERENCES [address] ([id])
);
GO
CREATE TABLE [dbo].[order_product](
	[order_id] INT,
	[product_id] INT,
	[quantity] tinyint,
	CONSTRAINT [pk_orderProduct] PRIMARY KEY ([order_id],[product_id]),
	CONSTRAINT [fk_orderProduct_order] FOREIGN KEY ([order_id])
	REFERENCES [order] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [fk_orderProduct_product] FOREIGN KEY ([product_id])
	REFERENCES [product] ([id])
);
GO
CREATE TABLE [dbo].[payment](
	[order_id] INT NOT NULL,
	[payment_type] INT,
	[user_id] INT NOT NULL,
	[total_amount] smallmoney,
	[date] timestamp,
	CONSTRAINT [pk_payment] PRIMARY KEY ([order_id]),
	CONSTRAINT [fk_payment_order] FOREIGN KEY ([order_id])
	REFERENCES [order] ([id])
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	CONSTRAINT [fk_payment_paymentType] FOREIGN KEY ([payment_type])
	REFERENCES [payment_type] ([id])
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
	CONSTRAINT [fk_payment_user] FOREIGN KEY ([user_id])
	REFERENCES [user] ([id])
	ON DELETE NO ACTION
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[menu](
	[id] INT IDENTITY(1,1) NOT NULL,
	[company_id] INT NOT NULL,
	[menu_name] NVARCHAR(30),
	[menu_image] NVARCHAR (MAX) NOT NULL,
	[total_price] smallmoney NOT NULL,
	[detail] text,
	CONSTRAINT [pk_menu] PRIMARY KEY ([id]),
	CONSTRAINT [fk_menu_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[menu_product](
	[menu_id] INT NOT NULL,
	[product_id] INT NOT NULL,
	[quantity] tinyint NOT NULL,
	[unit_price] smallmoney NOT NULL,
	CONSTRAINT [pk_menuProduct] PRIMARY KEY ([menu_id],[product_id]),
	CONSTRAINT [fk_menuProduct_menu] FOREIGN KEY ([menu_id])
	REFERENCES [menu] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT [fk_menuProduct_product] FOREIGN KEY ([product_id])
	REFERENCES [product] ([id])
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
);
GO
CREATE TABLE [dbo].[courier](
	[id] INT IDENTITY(1,1) NOT NULL,
	[company_id] INT,
	[name] NVARCHAR(30) NOT NULL,
	[surname] NVARCHAR(30),
	[phone] varchar(25) UNIQUE NOT NULL,
	CONSTRAINT [pk_courier] PRIMARY KEY ([id]),
	CONSTRAINT [fk_courier_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);
GO
CREATE TABLE [dbo].[delivery_status](
	[id] INT IDENTITY(1,1) NOT NULL,
	[status_name] NVARCHAR(20),
	CONSTRAINT [pk_deliveryStatus] PRIMARY KEY ([id]),
);
GO
CREATE TABLE [dbo].[delivery](
	[id] INT IDENTITY(1,1) NOT NULL,
	[company_id] INT ,
	[order_id] INT,
	[courier_id] INT,
	[address_id] INT,
	[status_id] INT,
	[release_date] datetime2,
	[arrival_date] datetime2,
	CONSTRAINT [pk_delivery] PRIMARY KEY ([id]),
	CONSTRAINT [fk_delivery_company] FOREIGN KEY ([company_id])
	REFERENCES [company] ([id])
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE,
	CONSTRAINT [fk_delivery_order] FOREIGN KEY ([order_id])
	REFERENCES [order] ([id]),
	CONSTRAINT [fk_delivery_courier] FOREIGN KEY ([courier_id])
	REFERENCES [courier] ([id]),
	CONSTRAINT [fk_delivery_address] FOREIGN KEY ([address_id])
	REFERENCES [address] ([id]),
	CONSTRAINT [fk_delivery_deliveryStatus] FOREIGN KEY ([status_id])
	REFERENCES [delivery_status] ([id])
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE,
);