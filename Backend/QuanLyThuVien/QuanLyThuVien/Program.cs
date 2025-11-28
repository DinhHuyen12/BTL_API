//<<<<<<< HEAD
//﻿//using BLL;
////using BLL.Interfaces;
////using DAL;
////using DAL.Interfaces;
////using DAL.Helper;

////var builder = WebApplication.CreateBuilder(args);


////builder.Services.AddControllers();

////// ✅ Đăng ký Dependency Injection
////builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
////builder.Services.AddScoped<IUserService, UserService>();    
////builder.Services.AddScoped<IAuthRepository, AuthRepository>();
////builder.Services.AddScoped<IUserRepository, UserRepository>();
////builder.Services.AddScoped<IDataHelper, DataHelper>();
////// 👇 Bật CORS
////builder.Services.AddCors(options =>
////{
////	options.AddPolicy("AllowAngularApp",
////		policy =>
////		{
////			policy.WithOrigins("http://127.0.0.1:5500") // frontend của bạn
////				  .AllowAnyHeader()
////				  .AllowAnyMethod()
////				  .AllowCredentials(); // nếu dùng cookie
////		});
////});


////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}
////// 👇 Enable CORS trước Authorization
////app.UseCors("AllowAngularApp");

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

////app.Run();
//=======
//﻿using baiapi1.DAL;
//using baiapi1.DAL.Interfaces;
//>>>>>>> 8cb58971d6e4003bd4811c344bcefad6ffb3b7d8
//using BLL;
//using BLL.Interfaces;
//using DAL;
//using DAL.Helper;
//<<<<<<< HEAD
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();

//// ===== Dependency Injection =====
//=======
//using DAL.Interfaces;
//using Helper;
//using Microsoft.Extensions.Configuration;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//>>>>>>> 8cb58971d6e4003bd4811c344bcefad6ffb3b7d8
//builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IDataHelper, DataHelper>();

//// ===== JWT Authentication =====
//var secretKey = "ThuVien2025_2025_SecretKey123!@#456-dfdfwer"; // giống DAL

//builder.Services.AddAuthentication(options =>
//{
//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//	options.RequireHttpsMetadata = false;
//	options.SaveToken = true;

//	options.TokenValidationParameters = new TokenValidationParameters
//	{
//		ValidateIssuer = false,
//		ValidateAudience = false,
//		ValidateIssuerSigningKey = true,
//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//		ClockSkew = TimeSpan.Zero // Không delay thời gian hết hạn
//	};
//});

//// ===== Authorization =====
//builder.Services.AddAuthorization();

//// ===== CORS =====
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowAngularApp",
//		policy =>
//		{
//			policy.WithOrigins(
//					"http://127.0.0.1:5500",
//					"http://localhost:5500",
//					"http://127.0.0.1:5501",
//					"http://localhost:5501"
//				)
//				.AllowAnyHeader()
//				.AllowAnyMethod()
//				.AllowCredentials();
//		});
//});

//<<<<<<< HEAD
//=======


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//>>>>>>> 8cb58971d6e4003bd4811c344bcefad6ffb3b7d8
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//IConfiguration configuration = builder.Configuration;
//var appSettingsSection = configuration.GetSection("AppSettings");
//builder.Services.Configure<AppSettings>(appSettingsSection);
//builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
//builder.Services.AddTransient<IBookRepository, BookRepository>();
//builder.Services.AddTransient<IBookCopiesRepository, BookCopiesRepository>();
//builder.Services.AddTransient<IBookCopiesBusiness, BookCopiesBusiness>();

//builder.Services.AddTransient<IShelvesRepository, BookshelvesRepository>();
//builder.Services.AddTransient<IBookshelvesBusiness, BookshelvesBusiness>();

//var app = builder.Build();

//// ===== SWAGGER =====
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//<<<<<<< HEAD
//// ===== Middlewares thứ tự BẮT BUỘC =====
//=======
//// 👇 Enable CORS trước Authorization
//>>>>>>> 8cb58971d6e4003bd4811c344bcefad6ffb3b7d8
//app.UseCors("AllowAngularApp");

//app.UseCors(x => x
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader());
//app.UseHttpsRedirection();

//app.UseAuthentication();   // << MUST HAVE
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using System.Text;
using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Interfaces;
using Helper; // nếu không cần bỏ hoặc đổi theo namespace thực tế của bạn
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using baiapi1.DAL.Interfaces;
using baiapi1.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ===== Dependency Injection =====
builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataHelper, DataHelper>();

// Nếu bạn dùng cả DatabaseHelper/IDatabaseHelper thì giữ đăng ký này (nếu không có thì xóa)
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();

// Các repository/business khác (giữ nếu các class/interface tồn tại)
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IBookCopiesRepository, BookCopiesRepository>();
builder.Services.AddTransient<IBookCopiesBusiness, BookCopiesBusiness>();

builder.Services.AddTransient<IShelvesRepository, BookshelvesRepository>();
builder.Services.AddTransient<IBookshelvesBusiness, BookshelvesBusiness>();

// ===== AppSettings binding =====
IConfiguration configuration = builder.Configuration;
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

// ===== JWT Authentication =====
// Lấy secret từ configuration nếu có, nếu không thì fallback về giá trị cứng
var secretFromConfig = configuration.GetValue<string>("AppSettings:Secret");
var secretKey = !string.IsNullOrWhiteSpace(secretFromConfig)
	? secretFromConfig
	: "ThuVien2025_2025_SecretKey123!@#456-dfdfwer";

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
		ClockSkew = TimeSpan.Zero // không delay thời gian hết hạn
	};
});

// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== CORS =====
// Chỉ đăng ký 1 policy chính. Nếu dùng cookie/credentials thì không dùng AllowAnyOrigin().
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp", policy =>
	{
		policy.WithOrigins(
				"http://127.0.0.1:5500",
				"http://localhost:5500",
				"http://127.0.0.1:5501",
				"http://localhost:5501"
			)
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials(); // nếu dùng cookie
	});
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== SWAGGER (chỉ dev) =====
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// ===== Middlewares (thứ tự quan trọng) =====
// Enable CORS trước Authentication/Authorization
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication(); // MUST HAVE trước UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
