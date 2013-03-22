#!/usr/bin/env python
# coding=utf-8
import os
import json
import io
import csv
import shutil
import sys, getopt
import itertools
import operator
import datetime
import time
#import pydoop.hdfs as hdfs

class get_tweetdetails:
    
    def get_userdetails(self, data):
        user_datainput = [{'username':record['from_user'], 'userid':record['from_user_id'], 'messageid':record['id'], 'message':record['text'], 'createddate':record['created_at'],'trend':data['query']} for record in data['results']]
        return user_datainput
    
    def get_geo(self, data):
        geo_data = [{'type':record['geo']['type'], 'lat':record['geo']['coordinates'][0], 'long':record['geo']['coordinates'][1], 'messageid':record['id']} for record in data['results'] if record['geo']]
        return geo_data

class tweet_extractor:

    global tweet_details
    tweet_details = get_tweetdetails()
    global createddatefortrend

    def extract_data(self):
	timeinterval = '';
	try:
		timeinterval = sys.argv[1];
	except:
		print 'required TimeStamp to create a Twitter Output'
		sys.exit(2)

        inputpath = '/data/Scraper/output/'
        outputpathforfileprocessedlogs = '/usr/local/source/fileprocessedlogs'
        listing = os.listdir(inputpath)
        listing.sort()
        hdfs_write = open("/data/" + timeinterval, mode='w')
        fileprocesstext_file = open(outputpathforfileprocessedlogs, "a+b")
        space = "\n"
        output = []
        origlist = []
        messageidtrenddict = {'messageid':'','message':'','userid':'','username':'','createddate':'','trend':''}
        geolist = []
        geo = {'type':'', 'lat':'', 'long':'', 'messageid':''}
        for pt in listing:
            data =  open(outputpathforfileprocessedlogs).read()
            if not pt in data:
                inputfilename=''.join([inputpath, pt])
                try:
                    tweet_data = json.load(open(inputfilename))
                except(ValueError):
                    print pt
                user_details = get_tweetdetails.get_userdetails(tweet_details, tweet_data)
                geo_details = get_tweetdetails.get_geo(tweet_details, tweet_data) 

                for x in user_details:
                    messageidtrenddict['username'] = x['username']
                    messageidtrenddict['userid'] = x['userid']
                    messageidtrenddict['message'] = x['message']
                    t = datetime.datetime.strptime(x['createddate'], '%a, %d %b %Y %H:%M:%S +0000')
                    d = t.strftime("%Y%m%d")
                    messageidtrenddict['createddate'] = d
                    messageidtrenddict['messageid'] = x['messageid']
                    messageidtrenddict['trend'] = x['trend'].lower()
                    origlist.append(messageidtrenddict.copy())

                for x in geo_details:
                    geo['type'] = x['type']
                    geo['lat'] = x['lat']
                    geo['long'] = x['long']
                    geo['messageid'] = x['messageid']
                    geolist.append(geo.copy())

                for i in origlist:
                        countFound = False
                        for j in geolist:
                                if i['messageid'] == j['messageid']:
                                        countFound = True
                                        break
                        if countFound == False:
                            print("Not Found", " ", i['userid'])
                            hdfs_write.write(''.join([str(i['messageid']),'\t',str(i['message'].encode('utf-8')).replace("	","t").strip(),'\t',str(i['userid']).strip(),'\t',str(i['username']).replace('\t',"").strip(),'\t',str(i['createddate']).strip(),'\t',str(i['trend']).replace("	","t").strip(),'\t',str('NULL').strip(),'\t',str('NULL'),'\t',str('NULL'),str(space)]))
                        else:                
                            print("Found", " ", j['lat'])
                            hdfs_write.write(''.join([str(i['messageid']),'\t',str(i['message'].encode('utf-8')).replace("	","t").strip(),'\t',str(i['userid']).strip(),'\t',str(i['username']).strip(),'\t',str(i['createddate']).strip(),'\t',str(i['trend']).replace("	","t").strip(),'\t',str(j['type']).strip(),'\t',str(j['lat']),'\t',str(j['long']),str(space)]))
                fileprocesstext_file.write(''. join([str(pt), str(space)]))
        hdfs_write.close()
        fileprocesstext_file.close()

if  __name__ == '__main__':
    the_tweet = tweet_extractor()
    the_tweet.extract_data()
