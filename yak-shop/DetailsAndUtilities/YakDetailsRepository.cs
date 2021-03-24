using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace yak_shop.DetailsAndUtilities
{
    public class YakDetailsRepository : IYakDetailsRepository
    {
        private IDbConnection db;

        public YakDetailsRepository(string connString)
        {
            this.db = new SqlConnection(connString);
        }

        public override List<YakDetails> GetAll()
        {
            return this.db.Query<YakDetails>("SELECT * FROM YakDetailsData").ToList();
        }

        public override void AddYak(YakDetails yak)
        {
            var sql = "INSERT INTO YakDetailsData (Name, Age, Sex, ageLastShaved) VALUES(@Name, @Age, @Sex, @ageLastShaved);" + "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.db.Query<int>(sql, yak).Single();
            yak.Id = id;
        }
        public override YakDetails FindYakData(int id)
        {
            return this.db.Query<YakDetails>("SELECT * FROM YakDetailsData WHERE Id = @Id", new { id }).SingleOrDefault();
        }

        public override void UpdateYakData(YakDetails yak)
        {
            var sql = "UPDATE YakDetailsData " + "SET Name = @Name, " + "Age = @Age, " + "Sex = @Sex, " + "ageLastShaved = @ageLastShaved " + "WHERE Id = @Id";
            this.db.Execute(sql, yak);
        }

        public override void RemoveYakData(int id)
        {
            this.db.Execute("DELETE FROM YakDetailsData WHERE Id = @Id", new { id });
        }
    }
}
