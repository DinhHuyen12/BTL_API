//using BLL;
//using BLL.Interfaces;
//using DAL;
//using DAL.Interfaces;
//using DAL.Helper;

//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers();

//// ✅ Đăng ký Dependency Injection
//builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
//builder.Services.AddScoped<IUserService, UserService>();    
//builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IDataHelper, DataHelper>();
//// 👇 Bật CORS
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowAngularApp",
//		policy =>
//		{
//			policy.WithOrigins("http://127.0.0.1:5500") // frontend của bạn
//				  .AllowAnyHeader()
//				  .AllowAnyMethod()
//				  .AllowCredentials(); // nếu dùng cookie
//		});
//});


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//// 👇 Enable CORS trước Authorization
//app.UseCors("AllowAngularApp");

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Interfaces;
using DAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ===== Dependency Injection =====
builder.Services.AddScoped<IAuthBusiness, AuthBusiness>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataHelper, DataHelper>();

// ===== JWT Authentication =====
var secretKey = "ThuVien2025_2025_SecretKey123!@#456-dfdfwer"; // giống DAL

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
		ClockSkew = TimeSpan.Zero // Không delay thời gian hết hạn
	};
});

// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== CORS =====
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp",
		policy =>
		{
			policy.WithOrigins(
					"http://127.0.0.1:5500",
					"http://localhost:5500",
					"http://127.0.0.1:5501",
					"http://localhost:5501"
				)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== SWAGGER =====
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// ===== Middlewares thứ tự BẮT BUỘC =====
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();   // << MUST HAVE
app.UseAuthorization();

app.MapControllers();

app.Run();
