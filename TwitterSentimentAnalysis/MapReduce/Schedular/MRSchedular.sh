#!/bin/bash
yesterday=$(date -d '1 day ago' +"%Y%m%d")
yesterday=20130213
#trend
bin/hadoop dfs -rmr /tweet/trend/$yesterday
bin/hadoop jar trendmr-0.0.1-SNAPSHOT.jar Trend /tweet/twitterdumps /tweet/trend/$yesterday $yesterday
#trendsentiment
bin/hadoop dfs -rmr /tweet/trendsentiment/$yesterday
bin/hadoop jar trendsentimentmr-0.0.1-SNAPSHOT.jar TrendSentiment /tweet/twitterdumps /tweet/trendsentiment/$yesterday $yesterday
#user
bin/hadoop dfs -rmr /tweet/user/$yesterday
bin/hadoop jar usermr-0.0.1-SNAPSHOT.jar User /tweet/twitterdumps /tweet/user/$yesterday $yesterday
#usersentiment
bin/hadoop dfs -rmr /tweet/usersentiment/$yesterday
bin/hadoop jar usersentimentmr-0.0.1-SNAPSHOT.jar UserSentiment /tweet/twitterdumps /tweet/usersentiment/$yesterday $yesterday
#geo
bin/hadoop dfs -rmr /tweet/geo/$yesterday
bin/hadoop jar geomr-0.0.1-SNAPSHOT.jar Geo /tweet/twitterdumps /tweet/geo/$yesterday $yesterday
#geosentiment
bin/hadoop dfs -rmr /tweet/geosentiment/$yesterday
bin/hadoop jar geosentimentmr-0.0.1-SNAPSHOT.jar GeoSentiment /tweet/twitterdumps /tweet/geosentiment/$yesterday $yesterday


