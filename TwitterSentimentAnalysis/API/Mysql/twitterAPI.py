#!/usr/bin/python
import web
import MySQLdb
import sys

urls = (
  '/trends', 'trends',
  '/trendsentiment','trendsentiment',
  '/users','users',
  '/usersentiment','usersentiment',
  '/geo','geo',
  '/geosentiment','geosentiment')

db = MySQLdb.connect(host="127.0.0.1",port=3306,user="root",passwd="root",db="test")

class trends:
    def GET(self):
        try:
            user_data = web.input(date="")
            query = "select * from Trend"
            if len(user_data.date)>0:
                query = "select * from Trend where date = "+user_data.date
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])

class trendsentiment:
    def GET(self):
        try:
            user_data = web.input(date="")
            query = "select * from Trendsentiment"
            if len(user_data.date)>0:
                query = "select * from Trendsentiment where date = "+user_data.date
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])
        
class users:
    def GET(self):
        try:
            user_data = web.input(date="",id="")
            query = "select * from Userdetails"
            if len(user_data.date)>0:
                query = "select * from Userdetails where date = "+user_data.date
            if len(user_data.id)>0:
                query = "select * from Userdetails where Userid = "+user_data.id
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])
        
class usersentiment:
    def GET(self):
        try:
            user_data = web.input(date="")
            query = "select * from Usersentiment"
            if len(user_data.date)>0:
                query = "select * from Usersentiment where date = "+user_data.date
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])
        
class geo:
    def GET(self):
        try:
            user_data = web.input(date="")
            query = "select * from Geo"
            if len(user_data.date)>0:
                query = "select * from test.Geo where date = "+user_data.date
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])
        
class geosentiment:
    def GET(self):
        try:
            user_data = web.input(date="")
            query = "select * from Geosentiment"
            if len(user_data.date)>0:
                query = "select * from test.Geosentiment where date = "+user_data.date
            print query
            cursor = db.cursor()
            cursor.execute(query)
            results = cursor.fetchall()
            return results
        except MySQLdb.Error, e:
            return "Error %d: %s" % (e.args[0], e.args[1])
               


if __name__ == "__main__":    
    app = web.application(urls, globals())
    app.run()