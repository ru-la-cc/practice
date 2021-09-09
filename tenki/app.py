from flask import Flask
from flask.templating import render_template
import sqlite3
import json
import requests

app = Flask(__name__)

@app.route("/")
def Helloworld():
	return "Hello Work 仕事探してます"

@app.route("/weather/")
def wheather():
	# 熊本は https://www.jma.go.jp/bosai/forecast/data/forecast/430000.json
	# https://www.jma.go.jp/bosai/forecast/data/forecast/{pathcode}.json
	res = requests.get("https://www.jma.go.jp/bosai/forecast/data/forecast/430000.json")
	tenki = json.loads(res.text)
	weather_string = tenki[0]["reportDatetime"][0:10].replace("-","/") + " "\
					+ tenki[0]["timeSeries"][0]["areas"][2]["area"]["name"] + " "\
					+ tenki[0]["timeSeries"][0]["areas"][2]["weathers"][0]
	return render_template("weather.html", tenki = weather_string)



if __name__ == "__main__":
	app.run(debug=True)
