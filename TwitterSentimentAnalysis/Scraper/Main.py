import twitter
import urllib2
import urllib
import time
import os
import json
import sys, getopt

#Constants
consumerKey = 'wqbWQTZwgBfVRXHJ6jImgw'
consumerSecret = 'q942QsSN2Blp9kqn1suZumkpv6OzKtO9qlJNUSCJw'
accessTokenKey = '627133469-d1SRK1BtKOseh5mOdwlKDizvacRaEieJTKQiNZgE'
accessTokenSecret = 'eatfrkdNmaJ7PHtrpCxIk3fKlsGhuMCvPM1QSPa8c'
woeid_India = 2367105
file_path = '/home/luser/Scraper/output/'
maxId_trend = {}
SEARCH_PATH = 'http://search.twitter.com/search.json'

def searchTwitter(term): 
    params = {'q' : term}
    if maxId_trend.has_key(term):
        params['since_id'] = maxId_trend[term]
    params['rpp'] = '100'
    path = "%s?%s" %(SEARCH_PATH, urllib.urlencode(params))
    print path
    content = ''
    try:
        resp = urllib2.urlopen(path)    
        content = resp.read()
    except:
        content = ''
    return content

def persistMaxidPerTerm(term, content):
    result = json.loads(content)
    maxId_trend[term] = result['max_id_str']

def getCurrentTime():
    return int(round(time.time() * 1000))

def start(interval):
    while True:
        country_woeid = ''
        try:
            country_woeid = sys.argv[1]
            print 'woeid = ' + country_woeid
        except:
            print 'required twitter woeid for anyone of the country'
            sys.exit(2)

        for trend in api.GetTrendsWoeid(country_woeid):
            tag = trend.name.replace('#', '')
            print tag
            content = searchTwitter(tag)
            if (len(content) > 0):
                persistMaxidPerTerm(tag,content)
                if not os.path.exists(file_path):
                    os.makedirs(file_path)
                f = file(file_path + str(getCurrentTime()),'w')
                f.write(content)
        time.sleep(float(interval))

api = twitter.Api(consumer_key=consumerKey, consumer_secret=consumerSecret, access_token_key=accessTokenKey, access_token_secret=accessTokenSecret)
start(360)
