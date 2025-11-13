
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ITIEntities>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddGoogle(o =>
{
    IConfiguration GoogleAuth = builder.Configuration.GetSection("Authentication:Google");
    o.ClientId = GoogleAuth["ClientId"];
    o.ClientSecret = GoogleAuth["ClientSecret"];
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.Password.RequireDigit = true)
    .AddEntityFrameworkStores<ITIEntities>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
