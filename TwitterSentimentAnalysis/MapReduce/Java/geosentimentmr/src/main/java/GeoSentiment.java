import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.*;

import org.apache.hadoop.fs.Path;
import org.apache.hadoop.conf.*;
import org.apache.hadoop.io.*;
import org.apache.hadoop.mapred.*;
import org.apache.hadoop.util.*;

public class GeoSentiment {
	public static class Map extends MapReduceBase implements
	Mapper<LongWritable, Text, Text, IntWritable> {
		private final static IntWritable one = new IntWritable(1);
		private Text word = new Text();
		private static String date;
		public void configure(JobConf job) {
			date = job.get("date");
		}
		public void map(LongWritable key, Text value,
				OutputCollector<Text, IntWritable> output, Reporter reporter)
				throws IOException {
			
			String line = value.toString();
			String[] items = line.split("\t");
			if (items.length == 9) {
				if(items[4].toString().trim().equals(date)){
				word.set(date+"\t"+items[7]+"_"+items[8] + "\t" + AnalyseMessage(items[1]));
				output.collect(word, one);
				}
			}
		}

		public static String AnalyseMessage(String msg) {
			try {
				String AnalysisUrl = "http://127.0.0.1:8080/message/";
				String sConfigFile = "conf/mapreduce.properties";
				
				InputStream in = GeoSentiment.class.getClassLoader().getResourceAsStream(sConfigFile);
				Properties props = new java.util.Properties();
				props.load(in);
							
				if(props.getProperty("analysisURL")!="")
					AnalysisUrl = props.getProperty("analysisURL");
								
				String value = msg;
				URL url = new URL(AnalysisUrl);
				HttpURLConnection httpCon = (HttpURLConnection) url
						.openConnection();
				httpCon.setDoInput(true);
				httpCon.setDoOutput(true);
				httpCon.setRequestMethod("POST");
				httpCon.setRequestProperty("Content-Type",
						"application/x-www-form-urlencoded");
				httpCon.setRequestProperty("Content-Length",
						"" + Integer.toString(value.getBytes().length));
				// Send request
				DataOutputStream wr = new DataOutputStream(
						httpCon.getOutputStream());
				wr.writeBytes(value);
				wr.flush();
				wr.close();

				// Get Response
				InputStream is = httpCon.getInputStream();
				BufferedReader rd = new BufferedReader(
						new InputStreamReader(is));
				String line;
				StringBuffer response = new StringBuffer();
				while ((line = rd.readLine()) != null) {
					response.append(line);
				}
				rd.close();

				return response.toString();
			} catch (Exception e) {
				return "Netural";
			}
		}
	}

	public static class Reduce extends MapReduceBase implements
			Reducer<Text, IntWritable, Text, IntWritable> {
		public void reduce(Text key, Iterator<IntWritable> values,
				OutputCollector<Text, IntWritable> output, Reporter reporter)
				throws IOException {
			int sum = 0;
			while (values.hasNext()) {
				sum += values.next().get();
			}
			output.collect(key, new IntWritable(sum));
		}
	}

	public static void main(String[] args) throws Exception {
		JobConf conf = new JobConf(GeoSentiment.class);
		conf.setJobName("GeoSentiment");
		conf.setOutputKeyClass(Text.class);
		conf.setOutputValueClass(IntWritable.class);
		conf.setMapperClass(Map.class);
		conf.setCombinerClass(Reduce.class);
		conf.setReducerClass(Reduce.class);
		conf.setInputFormat(TextInputFormat.class);
		conf.setOutputFormat(TextOutputFormat.class);
		conf.set("date", args[2]);
		
		FileInputFormat.setInputPaths(conf, new Path(args[0]));
		FileOutputFormat.setOutputPath(conf, new Path(args[1]));
		JobClient.runJob(conf);
	}
}
