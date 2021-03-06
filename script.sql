/****** Object:  Table [dbo].[InterviewDetails]    Script Date: 6/26/2018 11:28:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterviewDetails](
	[InterviewDetailId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](max) NOT NULL,
	[Designation] [nvarchar](max) NOT NULL,
	[InterviewDate] [date] NOT NULL,
	[InterviewTypeId] [int] NOT NULL,
	[HiringIndividualName] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_InterviewDetails] PRIMARY KEY CLUSTERED 
(
	[InterviewDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterviewTypes]    Script Date: 6/26/2018 11:28:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterviewTypes](
	[InterviewTypeId] [int] IDENTITY(1,1) NOT NULL,
	[InterviewTypeName] [nvarchar](max) NULL,
 CONSTRAINT [PK_InterviewTypes] PRIMARY KEY CLUSTERED 
(
	[InterviewTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[InterviewTypes] ON 

INSERT [dbo].[InterviewTypes] ([InterviewTypeId], [InterviewTypeName]) VALUES (1, N'Face to Face')
INSERT [dbo].[InterviewTypes] ([InterviewTypeId], [InterviewTypeName]) VALUES (2, N'Telephonic')
INSERT [dbo].[InterviewTypes] ([InterviewTypeId], [InterviewTypeName]) VALUES (3, N'Video Conference')
INSERT [dbo].[InterviewTypes] ([InterviewTypeId], [InterviewTypeName]) VALUES (4, N'Skype Interview')
SET IDENTITY_INSERT [dbo].[InterviewTypes] OFF
ALTER TABLE [dbo].[InterviewDetails]  WITH CHECK ADD  CONSTRAINT [FK_InterviewDetails_InterviewTypes] FOREIGN KEY([InterviewTypeId])
REFERENCES [dbo].[InterviewTypes] ([InterviewTypeId])
GO
ALTER TABLE [dbo].[InterviewDetails] CHECK CONSTRAINT [FK_InterviewDetails_InterviewTypes]
GO
