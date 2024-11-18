using CommunityToolkit.Mvvm.Messaging;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using DBDataGenerator.Messages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.Services
{
    /// <summary>
    /// 数据生成服务
    /// </summary>
    public class DataGenerateService
    {
        /// <summary>
        /// 一次生成自动生成多少条插入记录；根据电脑性能决定；例如为500条是，总共10000条数据要生成20次
        /// </summary>
        private int _onceGenerateCount = 100;

        /// <summary>
        /// 记录总共生成的记录，用户每次操作的时候回重置到0；方便外部显示，以及内部生成条码
        /// </summary>
        private int _generateCount = 0;

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 数据库服务
        /// </summary>
        private readonly DataBaseService _dataBaseService;

        public DataGenerateService(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }



        /// <summary>
        /// 生成并插入数据
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="tableName">表名</param>
        /// <param name="columnGenerateDataConfigs">列生成配置</param>
        /// <param name="count">数量</param>
        /// <returns>插入成功还是失败</returns>
        public async Task<bool> GenerateAndInsertData(string databaseName,string tableName, List<ColumnGenerateDataConfig> columnGenerateDataConfigs, int count = 1)
        {
            try
            {
                if (columnGenerateDataConfigs == null || columnGenerateDataConfigs.Count <= 0 || string.IsNullOrEmpty(tableName))
                {
                    return false;
                }

                // 重置生成条数
                this._generateCount = 0;

                // 使用对应数据库
                string useDatabaseQuery = $"USE {databaseName};";
                using (MySqlCommand useCommand = new MySqlCommand(useDatabaseQuery, this._dataBaseService.MySqlConnection))
                {
                    await useCommand.ExecuteNonQueryAsync();
                }

                // 分批插入数据到数据库，直到所有数据插入完成
                for (int i = 0; i < count; i += this._onceGenerateCount)
                {
                    string sql = GenerateInsertSql(tableName, columnGenerateDataConfigs, this._onceGenerateCount);
                    using (MySqlCommand command = new MySqlCommand(sql, this._dataBaseService.MySqlConnection))
                    {
                        //command.Parameters.AddWithValue("@value", value);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        WeakReferenceMessenger.Default.Send(new UpdateRealtimeInsertCountMsg(this._generateCount));// 发生插入了多少数据到VM
                    }

                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成批量插入sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnGenerateDataConfigs">列生成配置</param>
        /// <param name="count">插入数量</param>
        /// <returns>拼接好的sql语句;如果获取不到配置信息，直接返回null</returns>
        public string GenerateInsertSql(string tableName, List<ColumnGenerateDataConfig> columnGenerateDataConfigs, int count = 1)
        {
            if (columnGenerateDataConfigs == null || columnGenerateDataConfigs.Count <= 0 || string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            string insertPart = GenerateInsertPartSql(tableName, columnGenerateDataConfigs);
            string valuePart = GenerateValuePartSql(columnGenerateDataConfigs, count);

            return insertPart + valuePart;
        }


        /// <summary>
        /// 生成数据
        /// </summary>
        /// <param name="columnGenerateDataConfig"></param>
        /// <returns>返回对应配置生成的数据，如果配置为空，返回Null</returns>
        public string GenerateDataByColumnConfig(ColumnGenerateDataConfig columnGenerateDataConfig)
        {
            if (columnGenerateDataConfig == null)
            {
                return null;
            }

            string data = GenerateDataByDataGenerateConfig(columnGenerateDataConfig.DataGenerateType, columnGenerateDataConfig.DataGenerateConfig);

            return data;
        }


        /// <summary>
        /// 生成数据
        /// </summary>
        /// <param name="dataGenerateType">数据生成类型</param>
        /// <param name="dataGenerateConfig">数据生成配置</param>
        /// <returns>返回对应配置生成的数据，如果配置为空，返回Null</returns>
        public string GenerateDataByDataGenerateConfig(DataGenerateTypeEnum dataGenerateType, IDataGenerateConfig dataGenerateConfig)
        {
            // 自增ID不用生成
            if (dataGenerateConfig == null && dataGenerateType == DataGenerateTypeEnum.AutoIncrementID)
            {
                return null;
            }

            string data = null;
            switch (dataGenerateType)
            {
                case DataGenerateTypeEnum.AutoIncrementID: break;
                case DataGenerateTypeEnum.RandomInt:
                    {
                        RandomIntGenerateConfig config = (RandomIntGenerateConfig)dataGenerateConfig;
                        int minVal = config.LowerLimit;
                        int maxVal = config.UpperLimit;

                        data = random.Next(minVal, maxVal + 1).ToString();
                    }
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        RandomFloatGenerateConfig config = (RandomFloatGenerateConfig)dataGenerateConfig;
                        double minVal = config.LowerLimit;
                        double maxVal = config.UpperLimit;

                        double randomReal = minVal + random.NextDouble() * (maxVal - minVal);
                        data = randomReal.ToString();
                    }
                    break;
                case DataGenerateTypeEnum.JsonObject:
                    {
                        JsonObjectGenerateConfig config = (JsonObjectGenerateConfig)dataGenerateConfig;
                        data = JsonConvert.SerializeObject(GenerateJsonByConfig(config));
                    }
                    break;
                case DataGenerateTypeEnum.JsonArray:
                    {
                        JsonArrayGenerateConfig config = (JsonArrayGenerateConfig)dataGenerateConfig;
                        data = JsonConvert.SerializeObject(GenerateJsonArrayByConfig(config));
                    }
                    break;
                case DataGenerateTypeEnum.Barcode:
                    {
                        BarcodeGenerateConfig config = (BarcodeGenerateConfig)dataGenerateConfig;
                        data = config.Prefix + (config.SuffixNum + this._generateCount);
                    }
                    break;
                case DataGenerateTypeEnum.Guid:
                    {
                        data = Guid.NewGuid().ToString();
                    }
                    break;
                case DataGenerateTypeEnum.Datetime:
                    {
                        DatetimeGenerateConfig config = (DatetimeGenerateConfig)dataGenerateConfig;
                        data = config.DateTimeVal.ToString("G");
                    }
                    break;
                case DataGenerateTypeEnum.FixedString:
                    {
                        FixedStringGenerateConfig config = (FixedStringGenerateConfig)dataGenerateConfig;
                        data = config.StringVal;
                    }
                    break;
                default: break;
            };

            return data;
        }


        /// <summary>
        /// 生成insert部分语句
        /// </summary>
        /// <returns></returns>
        private string GenerateInsertPartSql(string tableName, List<ColumnGenerateDataConfig> columnGenerateDataConfigs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO `{tableName}` ( ");

            int index = 0;
            foreach (ColumnGenerateDataConfig item in columnGenerateDataConfigs)
            {
                if (index + 1 == columnGenerateDataConfigs.Count)
                {
                    // 遍历到最后一个字段时，不需要加逗号了
                    stringBuilder.Append($"`{item.ColumnName}`");
                }
                else
                {

                    stringBuilder.Append($"`{item.ColumnName}`,");
                }
                index++;
            }

            stringBuilder.Append($") VALUES ");

            return stringBuilder.ToString();
        }


        /// <summary>
        /// 生成value部分的sql
        /// </summary>
        /// <param name="columnGenerateDataConfigs"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private string GenerateValuePartSql(List<ColumnGenerateDataConfig> columnGenerateDataConfigs, int count = 1)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                string singleValuePart = $"(";

                int index = 0;
                foreach (ColumnGenerateDataConfig item in columnGenerateDataConfigs)
                {
                    string data = GenerateDataByColumnConfig(item);

                    if (data != null)
                    {
                        singleValuePart += $"'{GenerateDataByColumnConfig(item)}'";
                    }
                    else
                    {
                        singleValuePart += $"null";
                    }

                    if (index + 1 != columnGenerateDataConfigs.Count)
                    {
                        singleValuePart += ",";
                    }

                    index++;
                }

                // 判断是否已经生成到末尾
                if (i + 1 == count)
                {
                    singleValuePart += $");";
                }
                else
                {
                    singleValuePart += $"),";
                }

                stringBuilder.Append(singleValuePart);
                this._generateCount++;// 生成条数+1
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 根据配置，生成Json对象文本
        /// </summary>
        /// <param name="config">JSON生成配置</param>
        /// <returns></returns>
        private JObject GenerateJsonByConfig(JsonObjectGenerateConfig config)
        {
            // 创建一个 JSON 对象
            JObject jsonObject = new JObject();
            foreach (JsonPropertiesConfig item in config.Properties)
            {
                string data = GenerateDataByDataGenerateConfig(item.DataGenerateType, item.PropertyValueConfig);
                jsonObject[item.PropertyName] = data;
            }

            return jsonObject;
        }

        /// <summary>
        /// 根据配置，生成Json数组文本
        /// </summary>
        /// <param name="config">JSON Array生成配置</param>
        /// <returns></returns>
        private JArray GenerateJsonArrayByConfig(JsonArrayGenerateConfig config)
        {
            // 创建一个 JSON 数组
            JArray jsonArray = new JArray();
            if (config.JsonArrayCount < 0)
            {
                return jsonArray;
            }

            for (int i = 0; i < config.JsonArrayCount; i++)
            {
                jsonArray.Add(GenerateJsonByConfig(config.JsonObjectConfig));
            }

            return jsonArray;
        }
    }
}
