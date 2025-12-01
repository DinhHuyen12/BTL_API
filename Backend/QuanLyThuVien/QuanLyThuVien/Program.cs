/*using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using DAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Cấu hình CORS cho Angular
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp",
		policy => policy
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});

// đăng ký Database Helper
builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();

// đăng ký DI cho Business & Repository
builder.Services.AddScoped<IDataHelper, DataHelper>();
builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// JWT Authentication
//var secretKey = builder.Configuration["Jwt:Key"];
var secretKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(secretKey))
{
	throw new Exception("JWT Key is missing. Please add 'Jwt:Key' to appsettings.json");
}



var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
	};
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuanLyThuVien API", Version = "v1" });

	var jwtSecurityScheme = new OpenApiSecurityScheme
	{
		BearerFormat = "JWT",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = JwtBearerDefaults.AuthenticationScheme,
		Description = "Nhập JWT token vào đây",

		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};

	c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ jwtSecurityScheme, Array.Empty<string>() }
	});
});

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
*/




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

//using QuanLyThuVien.DAL;


var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//builder.Services.AddScoped<IUserRoleService, UserRoleService>();
//builder.Services.AddScoped<IRoleRepository, RoleRepository>();
//builder.Services.AddScoped<IRoleService, RoleService>();

//builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//builder.Services.AddScoped<IUserRoleService, UserRoleService>();
//builder.Services.AddScoped<DAL.Helper.IDataHelper, DAL.Helper.DataHelper>();
//builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();
//builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//builder.Services.AddScoped<IUserRoleService, UserRoleService>();




//// Add services to the container.
//builder.Services.AddControllers();

//// ===== Dependency Injection =====
//builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IDataHelper, DataHelper>();

//// Nếu bạn dùng cả DatabaseHelper/IDatabaseHelper thì giữ đăng ký này (nếu không có thì xóa)
//builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();

//// Các repository/business khác (giữ nếu các class/interface tồn tại)
//builder.Services.AddTransient<IBookRepository, BookRepository>();
//builder.Services.AddTransient<IBookCopiesRepository, BookCopiesRepository>();
//builder.Services.AddTransient<IBookCopiesBusiness, BookCopiesBusiness>();

//builder.Services.AddTransient<IShelvesRepository, BookshelvesRepository>();
//builder.Services.AddTransient<IBookshelvesBusiness, BookshelvesBusiness>();

// ===== DATABASE HELPER =====
builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddScoped<IDataHelper, DataHelper>();

// ===== AUTH, USER =====
builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ===== ROLE =====
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// ===== USER ROLE =====
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

// ===== OTHER REPOSITORIES =====
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookCopiesRepository, BookCopiesRepository>();
builder.Services.AddScoped<IBookCopiesBusiness, BookCopiesBusiness>();
builder.Services.AddScoped<IShelvesRepository, BookshelvesRepository>();
builder.Services.AddScoped<IBookshelvesBusiness, BookshelvesBusiness>();

// ===== CONTROLLERS =====
builder.Services.AddControllers();

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

//var app = builder.Build();

//// ===== SWAGGER (chỉ dev) =====
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//// ===== Middlewares (thứ tự quan trọng) =====
//// Enable CORS trước Authentication/Authorization
//app.UseCors("AllowAngularApp");

//app.UseHttpsRedirection();

//app.UseAuthentication(); // MUST HAVE trước UseAuthorization
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
try
{
	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseCors("AllowAngularApp");
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Console.WriteLine("🔥🔥🔥 STARTUP ERROR 🔥🔥🔥");
	Console.WriteLine(ex.ToString());
	Console.ReadLine();   // giữ console không tắt
	throw;
}
