using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DBDataGenerator.Common
{
    public class ColumnGenerateDataConfigJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<ColumnGenerateDataConfig>);
        }

        /// <summary>
        /// 反序列化JSON文本为
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JArray jsonArray = JArray.Load(reader);
            List<ColumnGenerateDataConfig> columnGenerateDataConfigs = new List<ColumnGenerateDataConfig>();

            // 遍历 jsonArray
            foreach (var item in jsonArray)
            {
                ColumnGenerateDataConfig columnGenerateDataConfig = new ColumnGenerateDataConfig();
                columnGenerateDataConfig.DatabaseName = item["DatabaseName"].ToString();
                columnGenerateDataConfig.TableName = item["TableName"].ToString();
                columnGenerateDataConfig.ColumnName = item["ColumnName"].ToString();

                // 解析数据类型分类
                int MysqlDataTypeCategoryEnumVal = int.Parse(item["MysqlDataTypeCategoryEnum"].ToString());
                columnGenerateDataConfig.MysqlDataTypeCategoryEnum = (MysqlDataTypeCategoryEnum)Enum.ToObject(typeof(MysqlDataTypeCategoryEnum), MysqlDataTypeCategoryEnumVal);

                // 解析数据生成类型
                int DataGenerateTypeEnumVal = int.Parse(item["DataGenerateType"].ToString());
                columnGenerateDataConfig.DataGenerateType = (DataGenerateTypeEnum)Enum.ToObject(typeof(DataGenerateTypeEnum), DataGenerateTypeEnumVal);

                // 解析配置
                string dataGenerateConfigStr = item["DataGenerateConfig"].ToString();
                columnGenerateDataConfig.DataGenerateConfig = this.ParseDataGenerateConfig(columnGenerateDataConfig.DataGenerateType,
                    dataGenerateConfigStr);

                columnGenerateDataConfigs.Add(columnGenerateDataConfig);
            }

            return columnGenerateDataConfigs;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 解析数据生成配置文本，返回对应类型的生成配置对象
        /// </summary>
        /// <param name="dataGenerateType">生成配置类型</param>
        /// <param name="data">原始数据文本</param>
        /// <returns>对应类型的配置对象</returns>
        private IDataGenerateConfig ParseDataGenerateConfig(DataGenerateTypeEnum dataGenerateType, string data)
        {
            IDataGenerateConfig dataGenerateConfig = null;
            if (data == null)
            {
                return dataGenerateConfig;
            }

            switch (dataGenerateType)
            {
                case DataGenerateTypeEnum.AutoIncrementID: dataGenerateConfig = null; break;
                case DataGenerateTypeEnum.RandomInt:
                    {
                        dataGenerateConfig = JsonConvert.DeserializeObject<RandomIntGenerateConfig>(data);
                    }
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        dataGenerateConfig = JsonConvert.DeserializeObject<RandomFloatGenerateConfig>(data);
                    }
                    break;
                case DataGenerateTypeEnum.JsonObject:
                    {
                        // JSON类型需要单独解析
                        dataGenerateConfig = this.ParseJsonPropertyGenerateConfig(DataGenerateTypeEnum.JsonObject, data);
                    }
                    break;
                case DataGenerateTypeEnum.JsonArray:
                    {
                        // JSON类型需要单独解析
                        dataGenerateConfig = this.ParseJsonPropertyGenerateConfig(DataGenerateTypeEnum.JsonArray, data);
                    }
                    break;
                case DataGenerateTypeEnum.Barcode:
                    {
                        dataGenerateConfig = JsonConvert.DeserializeObject<BarcodeGenerateConfig>(data);
                    }
                    break;
                case DataGenerateTypeEnum.Guid:
                    {
                        dataGenerateConfig = null;
                    }
                    break;
                case DataGenerateTypeEnum.Datetime:
                    {
                        dataGenerateConfig = JsonConvert.DeserializeObject<DatetimeGenerateConfig>(data);
                    }
                    break;
                case DataGenerateTypeEnum.FixedString:
                    {
                        dataGenerateConfig = JsonConvert.DeserializeObject<FixedStringGenerateConfig>(data);
                    }
                    break;
                default: dataGenerateConfig = null; break;
            };

            return dataGenerateConfig;
        }



        /// <summary>
        /// 解析类型为JSON生成
        /// </summary>
        /// <param name="dataGenerateType">生成类型</param>
        /// <param name="jsonPropertyStr">JSON属性原始数据</param>
        /// <returns></returns>
        private IDataGenerateConfig ParseJsonPropertyGenerateConfig(DataGenerateTypeEnum dataGenerateType, string jsonPropertyStr)
        {
            IDataGenerateConfig dataGenerateConfig = null;
            if (string.IsNullOrEmpty(jsonPropertyStr))
            {
                return dataGenerateConfig;
            }

            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(jsonPropertyStr);

            switch (dataGenerateType)
            {
                case DataGenerateTypeEnum.JsonObject:
                    {
                        JsonObjectGenerateConfig config = new JsonObjectGenerateConfig();
                        config.DatabaseName = jsonObject["DatabaseName"].ToString();
                        config.TableName = jsonObject["TableName"].ToString();
                        config.DatabaseName = jsonObject["ColumnName"].ToString();

                        // 解析属性
                        string properties = jsonObject["Properties"].ToString();
                        config.Properties = ParseJsonPropertiesConfig(properties);

                        dataGenerateConfig = config;
                    }
                    break;
                case DataGenerateTypeEnum.JsonArray:
                    {
                        JsonArrayGenerateConfig config = new JsonArrayGenerateConfig();
                        config.DatabaseName = jsonObject["DatabaseName"].ToString();
                        config.TableName = jsonObject["TableName"].ToString();
                        config.DatabaseName = jsonObject["ColumnName"].ToString();
                        config.JsonArrayCount = int.Parse(jsonObject["JsonArrayCount"].ToString());

                        // 解析里面JSON对象
                        string jsonObjectConfigStr = jsonObject["JsonObjectConfig"].ToString();
                        config.JsonObjectConfig = (JsonObjectGenerateConfig)ParseJsonPropertyGenerateConfig(DataGenerateTypeEnum.JsonObject, jsonObjectConfigStr);

                        dataGenerateConfig = config;
                    }
                    break;
                default: break;
            };


            return dataGenerateConfig;
        }

        /// <summary>
        /// 解析JSON属性集合
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns>JSON属性数据生成配置信息</returns>
        private List<JsonPropertiesConfig> ParseJsonPropertiesConfig(string data)
        {
            List<JsonPropertiesConfig> jsonPropertiesConfigs = new List<JsonPropertiesConfig>();
            if (string.IsNullOrEmpty(data))
            {
                return jsonPropertiesConfigs;
            }

            JArray? jsonArray = JsonConvert.DeserializeObject<JArray>(data);
            foreach (var item in jsonArray)
            {
                JsonPropertiesConfig config = new JsonPropertiesConfig();
                config.PropertyName = item["PropertyName"].ToString();

                // 解析数据生成类型
                int DataGenerateTypeEnumVal = int.Parse(item["DataGenerateType"].ToString());
                config.DataGenerateType = (DataGenerateTypeEnum)Enum.ToObject(typeof(DataGenerateTypeEnum), DataGenerateTypeEnumVal);

                config.PropertyValueConfig = ParseDataGenerateConfig(config.DataGenerateType, item["PropertyValueConfig"].ToString());

                jsonPropertiesConfigs.Add(config);
            }

            return jsonPropertiesConfigs;
        }
    }
}
