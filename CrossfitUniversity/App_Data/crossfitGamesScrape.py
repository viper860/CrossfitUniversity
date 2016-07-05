import requests, bs4, re, sqlite3 as lite, sys, os

# Parse individual stats
def parseRank():

	# Ranking URL
	url1 = "http://games.crossfit.com/scores/leaderboard.php?stage=0&sort=0&page="
	url2 = "&division=1&region=0&numberperpage=100&competition=0&frontpage=0&expanded=0&year=15&full=0&showtoggles=1&hidedropdowns=1&showathleteac=0&=&is_mobile=&scaled=0&fittest=1&fitSelect=0"
 
	conn = lite.connect('crossfit-open-2015.db')
	conn.text_factory = str
	cur = conn.cursor()
	#cur.execute("SELECT count(*) FROM athlete_info")
	cur.execute("SELECT overall_rank FROM athlete_info order by overall_rank desc limit 1")
	
	data=cur.fetchone()[0]
	if data == 0:
		pageNumber = 1
	else:
		pageNumber =int(data/100)
	print(data)
	print(pageNumber)
	#pageNumber = 1

	# Regular expressions to parse data
	reg1 = re.compile(">(.*) \(")
	reg2 = re.compile('a href="(.*)" target')
	reg3 = re.compile('\((.*)\)')
	reg12 = re.compile('top">(.*)</a>(\.*)')

	# Index
	index = 0

	while pageNumber <= 200:
		# Full URL
		url = url1 + str(pageNumber) + url2
		print(url)

		# Download rank page
		res = requests.get(url)
		res.raise_for_status()
		data = res.text
		data1 = data.encode('latin1').decode('utf8')
		soup = bs4.BeautifulSoup(data1,from_encoding='utf8')
#		soup = bs4.BeautifulSoup(res.text)
#		soup = bs4.BeautifulSoup(res.decode('utf-8', 'ignore'))
		info = soup.select('tr > td')

		# Parse page information
		while index < len(info):
			# Parse overall athlete rank
##			print(str(info[index]))
			overallRank = int(reg1.search(str(info[index])).group(1))

			# Parse athlete url
			athleteURL = reg2.search(str(info[index+1])).group(1)
			print("athleteURL: " + str(athleteURL))
			athleteNumber = int(re.compile('http://games.crossfit.com/athlete/(.*)').search(athleteURL).group(1))
			print("athleteNumber: " + str(athleteNumber))
#			print(info[index+1])
			athleteName = str(reg12.search(str(info[index+1])).group(1))
			print("athleteName: " + str(athleteName))

			# Event 1
			event1rank 	= int(reg1.search(str(info[index+2])).group(1))
			event1score	= int(reg3.search(str(info[index+2])).group(1))

			# Event 2
			event2rank 	= int(reg1.search(str(info[index+3])).group(1))
			event2score = int(reg3.search(str(info[index+3])).group(1))

			# Event 3
			event3rank 	= int(reg1.search(str(info[index+4])).group(1))
			event3score = int(reg3.search(str(info[index+4])).group(1))

			# Event 4
			event4rank 	= int(reg1.search(str(info[index+5])).group(1))
			event4score = int(reg3.search(str(info[index+5])).group(1))

			# Event 5
			event5rank 	= int(reg1.search(str(info[index+6])).group(1))
			event5score = int(reg3.search(str(info[index+6])).group(1))

			# Event 6
			event6rank 	= int(reg1.search(str(info[index+7])).group(1))
			event6score = seconds(str(reg3.search(str(info[index+7])).group(1)))
			#event6rank = int(0)
			#event6score=int(0)

			# Parse athlete
			parseAthleteStats(athleteURL)

			print("Rank: " + str(overallRank))
			print("Event 1: Rank "+str(event1rank) 	+ ", " + str(event1score))
			print("Event 2: Rank "+str(event2rank) 	+ ", " + str(event2score))
			print("Event 3: Rank "+str(event3rank) 	+ ", " + str(event3score))
			print("Event 4: Rank "+str(event4rank) 	+ ", " + str(event4score))
			print("Event 5: Rank "+str(event5rank) 	+ ", " + str(event5score))
			print("Event 6: Rank "+str(event6rank) 	+ ", " + str(event6score))

			# Iterate through athletes
			index += 8
			# Establish connection to sqlite db
			conn = lite.connect('crossfit-open-2015.db')
			conn.text_factory = str
			cur = conn.cursor()

			# Log to db
			try:
				cur.execute("UPDATE athlete_info SET athlete_name=?, overall_rank=?, event_one_rank=?, event_one_score=? ,event_two_rank=?, event_two_score=?, event_three_rank=?, event_three_score=?, event_four_rank=?, event_four_score=?, event_five_rank=?, event_five_score=?, event_six_rank=?, event_six_score=? WHERE athlete_number=?", [ athleteName, overallRank, event1rank, event1score, event2rank, event2score, event3rank, event3score, event4rank, event4score, event5rank, event5score, event6rank, event6score, athleteNumber])
				conn.commit()
			except lite.Error as e:
				if conn:
					conn.rollback()
				print("Error %s:" % e.args[0])
				sys.exit(1)
			finally:
				if conn:
					conn.close()

		# Iterate
		pageNumber += 1
		index = 0

# Pull individual statistics
def parseAthleteStats(athleteURL):
	# Download individual page
	res = requests.get(athleteURL)
	res.raise_for_status()
	soup = bs4.BeautifulSoup(res.text)
	dd = soup.select('dd')
	td = soup.select('td')
	lastDD = len(dd)
	if (str(soup.select("#page-title")).find('Not found') < 0):
		# Establish connection to sqlite db
		conn = lite.connect('crossfit-open-2015.db')
		conn.text_factory = str
		cur = conn.cursor()

		reg1 = re.compile('http://games.crossfit.com/athlete/(.*)')
		reg10 = re.compile('a href="/affiliate/(.*)"(\.*)')
		reg16 = re.compile('a href="/team/(.*)"(\.*)')
		reg11 = re.compile('">(.*)</a>(\.*)')
		reg13 = re.compile('<a href="(.*)/a>(\.*)')
		reg14 = re.compile('>(.*)<(\.*)')
		# Regular expression to parse text from <dd></dd>
		regDD = re.compile(r'<dd>(.*)</dd>')


		# Regular expression to parse text from <td></td>
		regTD = re.compile(r'<td>(.*)</td>')

		# Fill Gender
		if str(dd).find('Female') >= 0:
			gender = 'Female'
		else:
			gender = 'Male'

		# Fill age
		age 	= number(str(regDD.search(str(dd[lastDD-3])).group(1)))

		# Fill height
		height 	= inches(str(regDD.search(str(dd[lastDD-2])).group(1)))

		# Fill weight
		weight 	= lbkg(str(regDD.search(str(dd[lastDD-1])).group(1)))

		# Fran
		fran 	= seconds(str(regTD.search(str(td[ 1])).group(1)))

		# Helen
		helen	= seconds(str(regTD.search(str(td[ 3])).group(1)))

		# Grace
		grace	= seconds(str(regTD.search(str(td[ 5])).group(1)))

		# Filthy 50
		f50		= seconds(str(regTD.search(str(td[ 7])).group(1)))

		# Fight Gone Bad
		fgb		= number(str(regTD.search(str(td[ 9])).group(1)))

		# 400m Sprint
		fourH	= seconds(str(regTD.search(str(td[11])).group(1)))

		# 5k Run
		fiveK	= seconds(str(regTD.search(str(td[13])).group(1)))

		# Clean and Jerk
		cnj		= lbkg(str(regTD.search(str(td[15])).group(1)))

		# Snatch
		sn		= lbkg(str(regTD.search(str(td[17])).group(1)))

		# Deadlift
		dl 		= lbkg(str(regTD.search(str(td[19])).group(1)))

		# Back Squat
		bs 		= lbkg(str(regTD.search(str(td[21])).group(1)))

		# Max Pullups
		pu 		= number(str(regTD.search(str(td[23])).group(1)))
		#affiliateURL = str(str(regDD.search(str(dd[lastDD-5])).group(1))))
		#if str(regDD.search(str(dd[lastDD-5])).group(1))
		#if str(regDD.search(str(dd[lastDD-5])).group(1)):
		affiliateID = ''
		if str(dd[lastDD-5]).find('affiliate') >= 0:
			#print(str(dd[lastDD-5]))
			affiliateID = number(reg10.search(str(regDD.search(str(dd[lastDD-5])).group(1))).group(1))
		affiliateName = ''
		if str(dd[lastDD-5]).find('affiliate') >= 0:
			#print(str(dd[lastDD-5]))
			affiliateName = str(reg11.search(str(regDD.search(str(dd[lastDD-5])).group(1))).group(1))
###Team
		region = ''
		if str(dd[lastDD-7]).find('region') >= 0:
			#print(str(dd[lastDD-7]))
			#region = str(reg11.search(
			region =str(reg14.search(str(reg13.search(str(dd[lastDD-7])).group(1))).group(1))
		else:
				if str(dd[lastDD-6]).find('region') >= 0:
			#print(str(dd[lastDD-7]))
			#region = str(reg11.search(
					region =str(reg14.search(str(reg13.search(str(dd[lastDD-6])).group(1))).group(1))
			#.group(1))
			#)

		team = ''
		teamID = ''
		if str(dd[lastDD-6]).find('team') >= 0:
			#print(str(dd[lastDD-6]))
			#print str(regDD.search(str(dd[lastDD-6])).group(1))
			#region = str(reg11.search(

			try:
				team =str(reg14.search(str(reg13.search(str(dd[lastDD-6])).group(1))).group(1))
			except:
				print(str(dd[lastDD-6]))
				pass
			try:
				teamID =number(reg16.search(str(regDD.search(str(dd[lastDD-6])).group(1))).group(1))
			except:
				print(str(dd[lastDD-6]))
				pass
		else:
				if str(dd[lastDD-5]).find('team') >= 0:
					#print(str(dd[lastDD-5]))
			#region = str(reg11.search(
					try:
						team =str(reg14.search(str(reg13.search(str(dd[lastDD-5])).group(1))).group(1))
					except:
						print(str(dd[lastDD-5]))
						pass
					try:
						teamID =number(reg16.search(str(regDD.search(str(dd[lastDD-5])).group(1))).group(1))
					except:
						print(str(dd[lastDD-6]))
						pass


#<dd class="set_region_title_class north-east"><a href="/region/north-east">North East</a></dd>

		#print(str(dd[lastDD-9]))
		#print(str(dd[lastDD-8]))
		#print(str(dd[lastDD-7]))
		#print(str(dd[lastDD-6]))
		#print(str(dd[lastDD-5]))
		#print(str(dd[lastDD-4]))

		#print("//////////////////////////////////////////////")
#print(str(reg10.search(
		#print(str(regDD.search(str(dd[lastDD-5])).group(1)))
		#))
		print("//////////////////////////////////////////////")
		print("Athlete Number: " + reg1.search(athleteURL).group(1))
		print("Affiliate ID: " + str(affiliateID))
		print("Affiliate Name: " + str(affiliateName))
		print("Team ID: " + str(teamID))
		print("Team: " + str(team))
		print("Region: " + str(region))
		print("Gender: " + gender + " | Age: " + str(age) + " | Height: " + str(height) + " in | Weight: " + str(weight) + " lbs")

		print("Fran: " + str(fran))
		print("Helen: " + str(helen))
		print("Grace: " + str(grace))
		print("Filthy 50: " + str(f50))
		print("Fight Gone Bad: " + str(fgb))
		print("400m Sprint: " + str(fourH))
		print("5k: " + str(fiveK))

		print("Clean & Jerk: " + str(cnj))
		print("Snatch: " + str(sn))
		print("Deadlift: " + str(dl))
		print("Back Squat: " + str(bs))
		print("Max Pullups: " + str(pu))

		# Log to db
		try:
			cur.execute("INSERT or ignore INTO athlete_info (athlete_number, gender, age, height, weight, fran, helen, grace, filthy_50, fight_gone_bad, four_hundred_m, five_k, clean_and_jerk, snatch, deadlift, back_squat, max_pullups,  affiliate_id, affiliate_name, team_id, team, region) VALUES (?,?,?,?,?,?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",[int(reg1.search(athleteURL).group(1)), gender, age, height, weight, fran, helen, grace, f50, fgb, fourH, fiveK, cnj, sn, dl, bs, pu, affiliateID, affiliateName, teamID, team, region])
			conn.commit()
		except lite.Error as e:
			if conn:
				conn.rollback()
			print("Error %s:" % e.args[0])
			sys.exit(1)
		finally:
			if conn:
				conn.close()
	else:
		print("Not found")

# Subfunction to clean weight data
def lbkg(string):
	if string.find("lb") >= 0:
		# Weight is already in pounds, insert to db
		return int(string[:len(string)-3])
	elif string.find("kg") >= 0:
		# Convert weight from kilograms to pounds
		return int(string[:len(string)-3]) * 2.204
	else:
		return 0

# Subfunction to clean time data
def seconds(string):
	if string.find("--") >= 0:
		return 0
	else:
		reg = re.compile("(.*):(.*)(\.*)")
#		print(string)
#		print(reg.search(string).group(1))
#		print(reg.search(string).group(2))
		if (reg.search(string).group(1).find(":")) >= 0:
			string = str(reg.search(string).group(1))
		minutes = int(reg.search(string).group(1))
		seconds = int(reg.search(string).group(2))
		return (60*minutes) + seconds

# Subfunction to clean number data
def number(string):
	if string.find("--") >= 0:
		return 0
	else:
		return int(string)

# Subfunction to clean height
def inches(string):
	if string.find("--") >= 0:
		return 0
	elif string.find("cm") >= 0:
		return int(string[:len(string)-3]) / 2.54
	else:
		string = string[:len(string)-1]
		height = re.compile("(.*)'(.*)").search(string)
		return (int(str(height.group(1))) * 12) + int(str(height.group(2)))

#sys.stdout = open(os.devnull, "w")
parseRank()

#sys.stdout = sys.__stdout__
