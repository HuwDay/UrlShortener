

CREATE TABLE UrlKeys(
	UrlKey varchar(12) primary key not null,	-- primary key clustered index on the piece of information we want to most quickly search for
	FullUrl varchar(1700) unique not null		-- non-clustered index created by default. URL length of 1700 characters imposed due to inbuilt limit of non-clustered index
);


CREATE TABLE Log_Errors(
	ErrorID integer primary key not null identity(1,1),		-- primary key, auto-incrementing id number
	DateLogged datetime not null,							
	PageUrl varchar(2048) not null default(''),				-- varchar(2048), max length of a URL
	ErrorMessage varchar(3072) not null						-- varchar(3072), long enough to hold a really big stack trace
);