import web
from thrift import Thrift
from thrift.transport import TSocket
from thrift.transport import TTransport
from thrift.protocol import TBinaryProtocol
from hive_service import ThriftHive
from hive_service.ttypes import HiveServerException

urls = (
  '/getTrends', 'getTrends'
)

class getTrends:
    def GET(self):
        transport = TSocket.TSocket('192.168.10.155', 10000)
        transport = TTransport.TBufferedTransport(transport)
        protocol = TBinaryProtocol.TBinaryProtocol(transport)
        client = ThriftHive.Client(protocol)
        transport.open()
        client.execute("select * from twittertrends")
        trends = client.fetchAll()
        print trends
        transport.close()
        return trends

if __name__ == "__main__": 
    app = web.application(urls, globals())
    app.run()        

