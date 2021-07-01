using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dtest;
using MySqlConnector;

namespace Dtest
{
    public class DPostQuery
    {
        public AppDb Db { get; }

        public DPostQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<DPost> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Date`, `Discipline`, `Project`, `Status` FROM `DPost` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<DPost>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Date`, `Discipline`, `Project`, `Status` FROM `DPost` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `DPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<DPost>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<DPost>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new DPost(Db)
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetDateTime(1),
                        Discipline = reader.GetString(2),
                        Project = reader.GetString(3),
                        Status = reader.GetBoolean(4),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}