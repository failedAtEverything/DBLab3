using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApp.Cache;
using WebApp.Models;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<TvChannelContext>();
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddTransient<AppealCache>();
            builder.Services.AddTransient<EmployeeCache>();
            builder.Services.AddTransient<GenreCache>();
            builder.Services.AddTransient<ProgramCache>();
            builder.Services.AddTransient<WeeklyCache>();
            var app = builder.Build();
            app.UseSession();

            app.MapGet("/", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var result = new StringBuilder("");

                result.Append("<p><a href=\"/info\">Информация о подключении</a></p>");
                result.Append("<p><a href=\"/appeals\">Обращения</a></p>");
                result.Append("<p><a href=\"/employees\">Сотрудники</a></p>");
                result.Append("<p><a href=\"/genres\">Жанры</a></p>");
                result.Append("<p><a href=\"/programs\">Программы</a></p>");
                result.Append("<p><a href=\"/weekly\">Расписание</a>");
                result.Append("<p><a href=\"/searchform1\">Поиск (Cookie)</a></p>");
                result.Append("<p><a href=\"/searchform2\">Поиск (Session)</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/info", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var result = new StringBuilder("");

                foreach (var header in context.Request.Headers)
                {
                    result.Append($"<p>{header.Key}: {header.Value}</p>");
                }

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/appeals", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var dbContext = app.Services.GetService<TvChannelContext>();
                var cache = app.Services.GetService<AppealCache>();

                var result = new StringBuilder("<h3>Обращения</h3>");

                result.Append("<table BORDER=1>");
                result.Append("<tr>" 
                        + "<td>ID</td>"
                        + "<td>Full Name</td>"
                        + "<td>Organization</td>"
                        + "<td>Appeal Purpose</td>"
                        + "<td>Program ID</td>"
                    + "</tr>");

                var items = cache?.Get("recs");
                foreach (var item in items)
                {
                    result.Append("<tr>");
                    result.Append($"<td>{item.Id}</td>");
                    result.Append($"<td>{item.FullName}</td>");
                    result.Append($"<td>{item.Organization}</td>");
                    result.Append($"<td>{item.AppealPurpose}</td>");
                    result.Append($"<td>{item.ProgramId}</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/employees", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var dbContext = app.Services.GetService<TvChannelContext>();
                var cache = app.Services.GetService<EmployeeCache>();

                var result = new StringBuilder("<h3>Сотрудники</h3>");

                result.Append("<table BORDER=1>");
                result.Append("<tr>"
                        + "<td>ID</td>"
                        + "<td>Name</td>"
                        + "<td>Position</td>"
                    + "</tr>");

                var items = cache?.Get("recs");
                foreach (var item in items)
                {
                    result.Append("<tr>");
                    result.Append($"<td>{item.Id}</td>");
                    result.Append($"<td>{item.FullName}</td>");
                    result.Append($"<td>{item.Position}</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/genres", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var dbContext = app.Services.GetService<TvChannelContext>();
                var cache = app.Services.GetService<GenreCache>();

                var result = new StringBuilder("<h3>Жанры:</h3>");

                result.Append("<table BORDER=1>");
                result.Append("<tr>"
                        + "<td>ID</td>"
                        + "<td>Name</td>"
                        + "<td>Description</td>"
                    + "</tr>");

                var items = cache.Get("recs");
                foreach (var item in items)
                {
                    result.Append("<tr>");
                    result.Append($"<td>{item.Id}</td>");
                    result.Append($"<td>{item.Name}</td>");
                    result.Append($"<td>{item.Description}</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/programs", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var dbContext = app.Services.GetService<TvChannelContext>();
                var cache = app.Services.GetService<ProgramCache>();

                var result = new StringBuilder("<h3>Программы</h3>");

                result.Append("<table BORDER=1>");
                result.Append("<tr>"
                        + "<td>ID</td>"
                        + "<td>Name</td>"
                        + "<td>Length</td>"
                        + "<td>Rating</td>"
                        + "<td>GenreId</td>"
                        + "<td>Description</td>"
                    + "</tr>");

                var items = cache.Get("recs");
                foreach (var item in items)
                {
                    result.Append("<tr>");
                    result.Append($"<td>{item.Id}</td>");
                    result.Append($"<td>{item.Name}</td>");
                    result.Append($"<td>{item.Length}</td>");
                    result.Append($"<td>{item.Rating}</td>");
                    result.Append($"<td>{item.GenreId}</td>");
                    result.Append($"<td>{item.Description}</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/weekly", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var dbContext = app.Services.GetService<TvChannelContext>();
                var cache = app.Services.GetService<WeeklyCache>();

                var result = new StringBuilder("<h3>Список передач</h3>");

                result.Append("<table BORDER=1>");
                result.Append("<tr>"
                        + "<td>ID</td>"
                        + "<td>Week</td>"
                        + "<td>Mounth</td>"
                        + "<td>Year</td>"
                        + "<td>Start</td>"
                        + "<td>End</td>"
                        + "<td>Program ID</td>"
                        + "<td>Employee ID</td>"
                        + "<td>Guests</td>"
                    + "</tr>");

                var items = cache.Get("recs");
                foreach (var item in items)
                {
                    result.Append("<tr>");
                    result.Append($"<td>{item.Id}</td>");
                    result.Append($"<td>{item.WeekNumber}</td>");
                    result.Append($"<td>{item.WeekMonth}</td>");
                    result.Append($"<td>{item.WeekYear}</td>");
                    result.Append($"<td>{item.StartTime}</td>");
                    result.Append($"<td>{item.EndTime}</td>");
                    result.Append($"<td>{item.ProgramId}</td>");
                    result.Append($"<td>{item.EmployeesId}</td>");
                    result.Append($"<td>{item.Guests}</td>");
                    result.Append("</tr>");
                }

                result.Append("</table>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/searchform1", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8;";
                var inCookie = context.Request.Cookies;
                var outCookies = context.Response.Cookies;

                string len = inCookie["L"];
                string rat = inCookie["R"];
                string gen = inCookie["G"];

                var result = new StringBuilder("<h3>Поиск :</h3><form action=\"/searchform1\">");

                result.Append("<p>Введите продолжительность:</p>");

                if (len != null)
                {
                    result.Append($"<p><input name=\"L\" value=\"{len}\"></p>");
                }
                else
                {
                    result.Append($"<p><input name=\"L\"></p>");
                }
                result.Append("<p>Выбрать только передачи с рейтингом выше 50%</p>");

                if (rat == "YES")
                {
                    result.Append($"<p><input checked name=\"R\" type=\"radio\" value=\"YES\"></p>");
                }
                else
                {
                    result.Append($"<p><input name=\"R\" type=\"radio\" value=\"YES\"></p>");
                }


                result.Append("<p>Выберите жанр:</p>");
                result.Append("<select name=\"G\"");

                var items = app.Services.GetService<GenreCache>().Get("recs");
                foreach (var item in items)
                {
                    if (gen != null && item.Name == gen)
                    {
                        result.Append($"<option selected>{item.Name}<option>");
                    }
                    else
                    {
                        result.Append($"<option>{item.Name}<option>");
                    }
                }

                result.Append("</select>");
                result.Append("<input type=\"submit\" value=\"Отправить\"/></form>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                string L = context.Request.Query["L"];
                string R = context.Request.Query["R"];
                string G = context.Request.Query["G"];

                if (L is not null)
                {
                    outCookies.Append("L", context.Request.Query["L"]);
                }
                if (R is not null)
                {
                    outCookies.Append("R", context.Request.Query["R"]);
                }
                if (G is not null)
                {
                    outCookies.Append("G", context.Request.Query["G"]);
                }

                await context.Response.WriteAsync(result.ToString());
            });

            app.MapGet("/searchform2", async (context) =>
            {

                context.Response.ContentType = "text/html; charset=UTF-8;";
                var session = context.Session;

                string len = session.GetString("L");
                string rat = session.GetString("R");
                string gen = session.GetString("G");

                var result = new StringBuilder("<h3>Поиск :</h3><form action=\"/searchform2\">");

                result.Append("<p>Введите продолжительность:</p>");

                if (len != null)
                {
                    result.Append($"<p><input name=\"L\" value=\"{len}\"></p>");
                }
                else
                {
                    result.Append($"<p><input name=\"L\"></p>");
                }
                result.Append("<p>Выбрать только передачи с рейтингом выше 50%</p>");

                if (rat == "YES")
                {
                    result.Append($"<p><input checked name=\"R\" type=\"radio\" value=\"YES\"></p>");
                }
                else
                {
                    result.Append($"<p><input name=\"R\" type=\"radio\" value=\"YES\"></p>");
                }


                result.Append("<p>Выберите жанр:</p>");
                result.Append("<select name=\"G\"");

                var items = app.Services.GetService<GenreCache>().Get("recs");
                foreach (var item in items)
                {
                    if (gen != null && item.Name == gen)
                    {
                        result.Append($"<option selected>{item.Name}<option>");
                    }
                    else
                    {
                        result.Append($"<option>{item.Name}<option>");
                    }
                }

                result.Append("</select>");
                result.Append("<input type=\"submit\" value=\"Отправить\"/></form>");

                result.Append("<p><a href=\"/\">Назад</a></p>");

                string L = context.Request.Query["L"];
                string R = context.Request.Query["R"];
                string G = context.Request.Query["G"];

                if (L is not null)
                {
                    session.SetString("L", L);
                }
                if (R is not null)
                {
                    session.SetString("R", R);
                }
                if (G is not null)
                {
                    session.SetString("G", G);
                }

                await context.Response.WriteAsync(result.ToString());
            });

            app.Run();
        }
    }
}