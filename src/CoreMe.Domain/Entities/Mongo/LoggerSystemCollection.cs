using CoreMe.Domain.Constants;
using Serilog.Sinks.MongoDB;

namespace CoreMe.Domain.Entities.Mongo;

/// <summary>
/// 系统日志
/// </summary>
[MongoCollection(AppConst.SystemLogCollectionName)]
public class LoggerSystemCollection : LogEntry
{

}
