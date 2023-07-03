using System;

namespace IBM.XMS.Apps
{
    internal class ComTest
    {
        static void Main()
        {
            try
            {            
                ComTest xmsClient = new ComTest();
                Console.WriteLine("Auchan MQ Test");                
                xmsClient.SendMessage("DEV.QUEUE.1");
            }
            catch (XMSException e)
            {
                Console.WriteLine(e);
            }
        }

        private IConnection CreateConnection()
        {
            try
            {
                // Create ConnectionFactory
                IConnectionFactory cf = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ).CreateConnectionFactory();
                // Set the properties
                cf.SetStringProperty(XMSC.WMQ_HOST_NAME, "localhost");
                cf.SetIntProperty(XMSC.WMQ_PORT, 1414);
                cf.SetStringProperty(XMSC.WMQ_CHANNEL, "DEV.APP.SVRCONN");
                cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, "QM1");
                cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
                //cf.SetStringProperty(XMSC.USERID, "admin");
                //cf.SetStringProperty(XMSC.PASSWORD, "passw0rd");
                cf.SetIntProperty(XMSC.WMQ_CLIENT_RECONNECT_OPTIONS,XMSC.WMQ_CLIENT_RECONNECT);

                Console.WriteLine(cf);
                return cf.CreateConnection();
            }
            catch (XMSException e)
            {
                throw e;
            }
        }

        private ISession CreateSession(bool transacted)
        {
            IConnection connection = CreateConnection();
            connection.Start();
            return connection.CreateSession(transacted, AcknowledgeMode.AutoAcknowledge);
        }

        private IDestination AccessDestination(ISession session, String destName, bool isQueue = true)
        {
            if (isQueue)
                return session.CreateQueue(destName);
            else
                return session.CreateTopic(destName);
        }

        public void SendMessage(String destName, bool isQueue = true, bool transacted = false)
        {
            try
            {
                using (var session = CreateSession(transacted))
                {
                    IDestination destination = AccessDestination(session, destName, isQueue);
                    ITextMessage textMessage = session.CreateTextMessage("From XMS .NET Core application");
                    session.CreateProducer(destination).Send(textMessage);
                    Console.WriteLine("Message has been Sent Successfully");
                }
            }
            catch (XMSException e)
            {
                throw e;
            }
        }

        public void ReceiveMessage(String destName, bool isQueue = true, bool transacted = false)
        {
            try
            {
                using (var session = CreateSession(transacted))
                {
                    IDestination destination = AccessDestination(session, destName, isQueue);
                    Console.WriteLine(session.CreateConsumer(destination).ReceiveNoWait());
                }
            }
            catch (XMSException e)
            {
                throw e;
            }
        }
    }
}
