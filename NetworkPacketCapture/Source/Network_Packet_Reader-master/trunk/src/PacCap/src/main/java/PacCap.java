

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.nio.charset.Charset;
import org.apache.flume.Context;
import org.apache.flume.Event;
import org.apache.flume.EventDrivenSource;
import org.apache.flume.conf.Configurable;
import org.apache.flume.event.EventBuilder;
import org.apache.flume.source.AbstractSource;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import net.sourceforge.jpcap.capture.*;
import net.sourceforge.jpcap.net.*;

public class PacCap  extends AbstractSource implements Configurable, EventDrivenSource {
private static final Logger logger = LoggerFactory.getLogger(PacCap.class);
    
//    private static String msg;
    String capDevice;


    @Override
    public void configure(Context context) {
        
        capDevice = "eth0";
    }

    public static double[][] makeLocationsDoubleArray(String locations, String splitKey) {
        String[] locationArray = locations.split(splitKey);
        int locationCount = locationArray.length;
        double[][] result = new double[locationCount][locationCount];


        return new double[0][];  //To change body of created methods use File | Settings | File Templates.
    }

    @Override
    public void start() {
        log("Start\n");
        logger.info("Starting {}...", this);

        try {
            //if (adapter == null) {
                //adapter = new TwitterAdapter(getChannelProcessor(), track, count);
                //adapter.setOAuth(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            //}
            PacketCapture pcap = new PacketCapture();
            // Begin capture   
            pcap.open("eth0", true);
            CapturePacket t1 = new CapturePacket();
            pcap.addPacketListener(t1);
            pcap.capture(-1);
            log("Start two\n");
            //adapter.run();
        } catch (Exception e) {
            logger.error(e.getMessage(), e);
        }
        super.start();

        logger.info("Tweet source {} started.", getName());
    }

    @Override
    public void stop() {
        logger.info("Avro source {} stopping: {}", getName(), this);
        
        super.stop();
    }

    public static void log(String msg) {
        try {
            BufferedWriter out = new BufferedWriter(new FileWriter("/home/hduser/capture.log", true));
            out.write(msg);
            out.close();
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }

    }
    
    public class CapturePacket implements RawPacketListener, PacketListener {

        public void rawPacketArrived(RawPacket rawPacket) {
            //System.out.println("rawPacket="+rawPacket);   
        }

        public void packetArrived(Packet packet) {

            try {

                if (packet instanceof TCPPacket) {

                    TCPPacket tcppacket = ((TCPPacket) packet);

                    //if( tcppacket.getDestinationPort()==110 || tcppacket.getSourcePort() ==110) // pop3   

                    if (tcppacket.getDestinationPort() == 80 || tcppacket.getSourcePort() == 80) { // http   

                        String captureStr = new String(tcppacket.getTCPData());
                        String srcHost = tcppacket.getSourceAddress();
                        String dstHost = tcppacket.getDestinationAddress();

                        if (!captureStr.isEmpty()) {

                            String hostAndRequest = srcHost + "   " + dstHost + "    " + captureStr;
                            System.out.println(hostAndRequest);
                            Event e = EventBuilder.withBody(hostAndRequest, Charset.forName("UTF-8"));
                            log(e.toString());
                            getChannelProcessor().processEvent(e);
                        }
                        //fos.close();   
                    }
                }
            } catch (Exception ioe) {
                System.out.println("Exception ocurred:" + ioe);
            }
        }
    }
}
