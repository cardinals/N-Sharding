﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSharding.Sharding.Rule
{
    /// <summary>
    /// 哈希数据库分库算法
    /// </summary>
    public class DatabaseShardingHashAlgorithm : ISingleKeyDatabaseShardingAlgorithm
    {
        private int hashFactor;

        public DatabaseShardingHashAlgorithm(int hashFactor)
        {
            this.hashFactor = hashFactor;
        }

        public string DoEqualSharding(List<string> availableTargetNames, ShardingValue shardingValue)
        {
            var bytes = Encoding.UTF8.GetBytes(shardingValue.Value);
            var longValue = BitConverter.ToInt64(bytes, 0);
            var result = longValue % hashFactor;

            return availableTargetNames.FirstOrDefault(i => i.EndsWith(result.ToString()));
        }

        public List<string> DoInSharding(List<string> availableTargetNames, ShardingValue shardingValue)
        {
            var result = new List<string>();
            foreach (var dbName in availableTargetNames)
            {
                foreach (var value in shardingValue.Values)
                {
                    var bytes = Encoding.UTF8.GetBytes(shardingValue.Value);
                    var longValue = BitConverter.ToInt64(bytes, 0);
                    var modResult = longValue % hashFactor;

                    if (dbName.EndsWith(modResult.ToString()))
                    {
                        result.Add(dbName);
                    }
                }
            }

            return result;
        }

        public List<string> DoBetweenSharding(List<string> availableTargetNames, ShardingValue shardingValue)
        {
            throw new NotImplementedException();
        }
    }
}
