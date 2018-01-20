using OwinSelfHostSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{
    class RelocationHandler
    {
        //public void Reshard()
        //{
            //GetDataForRelocation();// compareTable();
            //RelocationData();
        //}
        public void Reshard()
        {
            var oldTable = Storage.LoadBucketShardTable("old_bucket_shard.txt");
            foreach (var row in oldTable)
            {
                Console.WriteLine("old shard " + row.Value);
                Console.WriteLine("new shard " + Storage.bucketShardTable[row.Key]);
                if (row.Value != Storage.bucketShardTable[row.Key])
                {
                    new ProxyController().SendOn(Storage.keyBucketTable[row.Key], row.Value, Storage.bucketShardTable[row.Key]);
                    Console.WriteLine("");
                    //Reshard(ProxyStorage.bucketShardTable[row.Key], row.Value, FindRowsFromBucket(row.Key));
                    //bst.ChangeShard(row.Key, BSTable[row.Key]);
                    //не дописывает
                }
            }
            //сравнить две таблицы, по бакету взять список айди, которые не там
            //и переместить их
        }

        //сделать так, чтобы он завершился, его дождались все
        //private void RelocationData(List<int> ids, int oldShard, int newShard)
        //{ 
        //    ValuesController client = new ValuesController();
        //    ////////вот тут, надо отправить из прокси на перенос
        //    foreach (var id in ids){
        //        //client.url = new ValuesController().getUrl(id);
        //        var value = client.Get(id);
        //        client.Put(id,value);
        //        client.Delete(id);
        //    }
        //}
    }
}
