import keyboard
import pydirectinput
from os.path import expanduser

print(expanduser(r'~\Documents'))

def print_pressed_keys(e):
	try:
		with open(expanduser(r'~\Documents') + '/config.properties', 'r') as file_handler:
		    for line in file_handler:
		    	if UPDATE_VALUE(line.split(' ')[1]) == e.name and e.event_type == 'down':
		        	print(">> Start shortcut process...")
		        	for i in range(2, len(line.split(' '))):
		        		pydirectinput.press(UPDATE_VALUE(line.split(' ')[i]))
		        		print(">> Pressed {0}".format(line.split(' ')[i]))
	
	except:
		print(">> An error occured!")

def UPDATE_VALUE(KEY):
	KEY = KEY.lower()

	if KEY == "spacebar":
		KEY = "space"
	elif KEY == "escape":
		KEY = "esc"
	elif len(KEY) == 2 and KEY[0] == "d":
		KEY = KEY[1]

	return KEY


keyboard.hook(print_pressed_keys)
keyboard.wait()