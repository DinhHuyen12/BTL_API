using System.Data;
using System.Data.SqlClient;

namespace DAL.Helper
{
	public interface IDataHelper
	{
		bool Open();
		void Close();

		DataTable ExcuteReader(string procedureName, params object[] paramList);

		Dictionary<string, object> ExcuteNonQuery(string procedureName, Dictionary<string, object> inputParams, List<SqlParameter> outputParams = null);
	}
}
