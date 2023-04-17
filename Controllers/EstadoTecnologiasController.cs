using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Data;
using WebApp.Models.ApplicationModels;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
    public class EstadoTecnologiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public object MessageBox { get; private set; }

        public EstadoTecnologiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoTecnologias
        public async Task<IActionResult> Index()
        {
            return _context.EstadosTecnologia != null ?
                        View(await _context.EstadosTecnologia.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.EstadosTecnologia'  is null.");
        }

        [HttpGet("/EstadoTecnologias/Test/{tecnologiaId}")]
        public async Task<EstadoTecnologia> TestTecnologia(int tecnologiaId)
        {
            var tecnologias = await _context.Tecnologias.Include(t => t.Tipo).FirstOrDefaultAsync(m => m.Id == tecnologiaId);
            HttpClient client = new HttpClient();
            var jsonResponse = "";
            var estadoTecnologia = new EstadoTecnologia();//trabalhar com o estadoTecnologia pq são esses os campos do EstadoTecnologias
            estadoTecnologia.IdTecnologia = tecnologiaId;
            estadoTecnologia.NameTecnologia = tecnologias.Name;
            estadoTecnologia.Timestamp = DateTime.Now;
            try
            {
                var user = "user_for_tests";
                var pwd = "HYj56@eeRy";
                var res_query = -1;
                switch (tecnologias.TypeId)
                {
                    //Bases Dados ORACLE
                    case 1:
                        var EXAMPLE_CONNECTION = "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;Integrated Security=no;";

                        if (tecnologias != null)
                        {
                            tecnologias.Link = tecnologias.Link.Replace("USER_REPLACE_ME", user).Replace("PASSWORD_REPLACE_ME", pwd);
                            var test_query_oracle = "SELECT * FROM XXX WHERE rownum = 1";
                            using (OracleConnection connection = new OracleConnection(tecnologias.Link))
                            {
                                try
                                {
                                    connection.Open();
                                    Console.WriteLine("Connection established!");
                                    OracleCommand command = new OracleCommand(test_query_oracle, connection);
                                    OracleDataReader reader = command.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        Console.WriteLine(reader.GetString(0));
                                    }

                                    reader.Close();
                                }
                                catch (Exception ex)
                                {

                                    Console.WriteLine("Failed to connect to database: " + ex.Message);
                                }
                            }
                        }

                        break;
                    //Bases Dados SQL Server
                    case 2:
                        var test_query_sql_server = "SELECT TOP(1) Id FROM Tipos";
                        if (tecnologias != null)
                        {
                            tecnologias.Link = tecnologias.Link.Replace("USER_REPLACE_ME", user).Replace("PASSWORD_REPLACE_ME", pwd);
                            using (SqlConnection connection = new SqlConnection(tecnologias.Link))
                            {
                                SqlCommand command = new SqlCommand(test_query_sql_server, connection);
                                command.Connection.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                try
                                {
                                    while (reader.Read())
                                    {
                                        var id = reader["id"];
                                        res_query = id != null ? Int32.Parse(reader["id"].ToString()) : -1;
                                    }
                                }
                                finally
                                {
                                    // Always call Close when done reading.
                                    reader.Close();
                                }
                            }
                        }
                        var oldEstadoTecnologiasqlserver = await _context.EstadosTecnologia
               .Where(x => x.IdTecnologia == tecnologiaId)
               .OrderByDescending(m => m)
               .FirstOrDefaultAsync();
                        Console.WriteLine(oldEstadoTecnologiasqlserver);
                        // DB OK
                        if (res_query != -1)
                        {
                            estadoTecnologia.Ok = true;
                            estadoTecnologia.StatusCode = 200;
                            estadoTecnologia.Message = "Bases Dados SQL a funcionar perfeitamente";
                            if (oldEstadoTecnologiasqlserver != null)
                            {
                                if (oldEstadoTecnologiasqlserver.Ok != estadoTecnologia.Ok && oldEstadoTecnologiasqlserver.StatusCode != estadoTecnologia.StatusCode && oldEstadoTecnologiasqlserver.Message != estadoTecnologia.Message)
                                {
                                    _context.Add(estadoTecnologia);
                                }
                            }
                            else
                            {
                                _context.Add(estadoTecnologia);
                            }
                        }
                        // DB NOT OK
                        else
                        {
                            estadoTecnologia.Ok = false;
                            estadoTecnologia.StatusCode = 000;
                            estadoTecnologia.Message = "ERRO, Bases Dados ORACLE não encontrada";
                        }
                        break;
                    //Servidores Aplicacionais/Aplicações
                    case 3:
                    case 4: 
                        using (HttpResponseMessage response = new HttpResponseMessage())
                        {
                            try
                            {
                                await client.GetAsync(tecnologias.Link);
                                jsonResponse = await response.Content.ReadAsStringAsync();
                                estadoTecnologia.StatusCode = (int)response.StatusCode;
                                if (estadoTecnologia.StatusCode >= 200 && estadoTecnologia.StatusCode <= 299)
                                {
                                    estadoTecnologia.Ok = true;
                                    estadoTecnologia.Message = "Ok";
                                }
                            }
                            catch (Exception e)
                            {
                                estadoTecnologia.Message = e.Message;
                            }
                        }
                        break;
                    default:
                        break;
                }
                var oldEstadoTecnologia = await _context.EstadosTecnologia
                .Where(x => x.IdTecnologia == tecnologiaId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
                Console.WriteLine(oldEstadoTecnologia);
                if (oldEstadoTecnologia != null)
                {
                    if ((estadoTecnologia.Ok != oldEstadoTecnologia.Ok) || (estadoTecnologia.StatusCode != oldEstadoTecnologia.StatusCode) || (estadoTecnologia.Message != oldEstadoTecnologia.Message))
                    {
                        _context.Add(estadoTecnologia);
                    }
                    oldEstadoTecnologia.Timestamp = DateTime.Now;
                    _context.Entry(oldEstadoTecnologia).State = EntityState.Modified;
                }
                else
                {
                    _context.Add(estadoTecnologia);
                }
                
                await _context.SaveChangesAsync();
                return estadoTecnologia;
            }
            catch (Exception e)
            {
                //A tipologia não existe 
                throw ;
            }
        }
        //---------------para fazer aquilo de apagar os estados ao apagar as tecs
        [HttpGet("/EstadoTecnologias/Delete/{tecnologiaId}")]
        public async Task<EstadoTecnologia> DeleteTecnologia(int tecnologiaId)
        {
            var tecnologias = await _context.Tecnologias.Where(m => m.Id == tecnologiaId).ToListAsync();
            HttpClient client = new HttpClient();
            //var jsonResponse = "";
            var estadoTecnologia = new EstadoTecnologia();//trabalhar com o estadoTecnologia pq são esses os campos do EstadoTecnologias
            //estadoTecnologia.IdTecnologia = tecnologiaId;
            //estadoTecnologia.NameTecnologia = tecnologias.Name;
            //estadoTecnologia.Timestamp = DateTime.Now;
            try
            {
                //var user = "user_for_tests";
                //var pwd = "HYj56@eeRy";
                //var res_query = -1;

                //var oldEstadoTecnologia = await _context.EstadosTecnologia
                //.Where(x => x.IdTecnologia == tecnologiaId)
                //.OrderByDescending(m => m.Timestamp)
                //.FirstOrDefaultAsync();
                //Console.WriteLine(oldEstadoTecnologia);
                //if (oldEstadoTecnologia != null)
                //{
                //    if ((estadoTecnologia.Ok != oldEstadoTecnologia.Ok) || (estadoTecnologia.StatusCode != oldEstadoTecnologia.StatusCode) || (estadoTecnologia.Message != oldEstadoTecnologia.Message))
                //    {
                //        _context.Add(estadoTecnologia);
                //    }
                //    oldEstadoTecnologia.Timestamp = DateTime.Now;
                //    _context.Entry(oldEstadoTecnologia).State = EntityState.Modified;
                //}
                //else
                //{
                //    _context.Add(estadoTecnologia);
                //}
                //_context.Add(estadoTecnologia);
                await _context.SaveChangesAsync();
                return estadoTecnologia;
            }
            catch (Exception e)
            {
                //A tipologia não existe 
                throw;
            }
        }

        // GET: EstadoTecnologias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstadosTecnologia == null)
            {
                return NotFound();
            }

            var estadoTecnologia = await _context.EstadosTecnologia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoTecnologia == null)
            {
                return NotFound();
            }

            return View(estadoTecnologia);
        }

        // GET: EstadoTecnologias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoTecnologias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdTecnologia,ADUp,DBUp,Ok,StatusCode,Message,Timestamp")] EstadoTecnologia estadoTecnologia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoTecnologia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoTecnologia);
        }

        // GET: EstadoTecnologias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstadosTecnologia == null)
            {
                return NotFound();
            }

            var estadoTecnologia = await _context.EstadosTecnologia.FindAsync(id);
            if (estadoTecnologia == null)
            {
                return NotFound();
            }
            return View(estadoTecnologia);
        }

        // POST: EstadoTecnologias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdTecnologia,ADUp,DBUp,Ok,StatusCode,Message,Timestamp")] EstadoTecnologia estadoTecnologia)
        {
            if (id != estadoTecnologia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoTecnologia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoTecnologiaExists(estadoTecnologia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estadoTecnologia);
        }

        // GET: EstadoTecnologias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstadosTecnologia == null)
            {
                return NotFound();
            }

            var estadoTecnologia = await _context.EstadosTecnologia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoTecnologia == null)
            {
                return NotFound();
            }

            return View(estadoTecnologia);
        }

        // POST: EstadoTecnologias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstadosTecnologia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EstadosTecnologia'  is null.");
            }
            var estadoTecnologia = await _context.EstadosTecnologia.FindAsync(id);
            if (estadoTecnologia != null)
            {
                _context.EstadosTecnologia.Remove(estadoTecnologia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoTecnologiaExists(int id)
        {
            return (_context.EstadosTecnologia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
