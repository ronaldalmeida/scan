
CREATE DATABASE [Test]

GO

CREATE TABLE [dbo].[Book](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Author] [nvarchar](100) NULL,
	[Title] [nvarchar](100) NULL,
	[PageCount] [int] NULL,
	[comment] [nvarchar](200) NULL,
	[RDStatus] [nvarchar](10) NULL,
	[isbn_10] [nvarchar](10) NULL,
	[isbn_13] [bigint] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Book] ADD  DEFAULT ('Unread') FOR [RDStatus]

GO


