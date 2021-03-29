using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using yak_shop.Models;

namespace yak_shop.DetailsAndUtilities
{
    public class YakDetailsRepository : IYakDetailsRepository
    {
        private IDbConnection db;
        private readonly YakContext _context;

        public YakDetailsRepository(string connString)
        {
            this.db = new SqlConnection(connString);
        }

        public YakDetailsRepository(YakContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public YakDetails GetYak(int yakId)
        {
            //Console.WriteLine("Testing");
            //if (yakId == null)
            //throw new ArgumentNullException(nameof(yakId));
            return this.db.Query<YakDetails>("SELECT * FROM YakDetailsData WHERE Id = @Id", new { yakId }).SingleOrDefault();
            //return _context.YakItems.Where(y => y.Id == yakId).FirstOrDefault();
        }

        //public List<YakDetails> GetAll()
        //{
        //    return this.db.Query<YakDetails>("SELECT * FROM YakDetailsData").ToList();
        //}

        //public void AddYak(YakDetails yak)
        //{
        //    var sql = "INSERT INTO YakDetailsData (Name, Age, Sex, ageLastShaved) VALUES(@Name, @Age, @Sex, @ageLastShaved);" + "SELECT CAST(SCOPE_IDENTITY() as int)";
        //    var id = this.db.Query<int>(sql, yak).Single();
        //    yak.Id = id;
        //}
        //public YakDetails FindYakData(int id)
        //{
        //    return this.db.Query<YakDetails>("SELECT * FROM YakDetailsData WHERE Id = @Id", new { id }).SingleOrDefault();
        //}

        //public void UpdateYakData(YakDetails yak)
        //{
        //    var sql = "UPDATE YakDetailsData " + "SET Name = @Name, " + "Age = @Age, " + "Sex = @Sex, " + "ageLastShaved = @ageLastShaved " + "WHERE Id = @Id";
        //    this.db.Execute(sql, yak);
        //}

        //public void RemoveYakData(int id)
        //{
        //    this.db.Execute("DELETE FROM YakDetailsData WHERE Id = @Id", new { id });
        //}
    }
}
