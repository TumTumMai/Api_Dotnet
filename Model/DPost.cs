using System.Data;
using System.Threading.Tasks;
using Dtest;
using MySqlConnector;

namespace Dtest
{
    public class DPost
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Discipline { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }


        internal AppDb Db { get; set; }

        public DPost()
        {
        }

        internal DPost(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `DPost` (`Date`, `Discipline`, `Project`, `Status`) VALUES (@date, @discipline, @Project, @Status);";
            DParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `DPost` SET `Date` = @date, `Discipline` = @discipline, `Project` = @project, `Status` = @status WHERE `Id` = @id;";
            DParams(cmd);
            DId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `DPost` WHERE `Id` = @id;";
            DId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void DId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void DParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date",
                DbType = DbType.String,
                Value = Date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@discipline",
                DbType = DbType.String,
                Value = Discipline,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@project",
                DbType = DbType.String,
                Value = Project,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@status",
                DbType = DbType.String,
                Value = Status,
            });
        }
    }
}