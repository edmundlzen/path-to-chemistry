extends Node2D

const fileDir = "user://Data/"
var filePath = fileDir+"Save.dat"
var dir = Directory.new()
var file = File.new()
var Data = {
	"Slot1": "1", "Slot2": "2", "Slot3":"3", "Slot4":"4",
	"Slot5": "5", "Slot6": "6", "Slot7":"7", "Slot8":"8",
}

func Save():
	if !dir.dir_exists(fileDir):
		dir.make_dir_recursive(fileDir)
	file.open(filePath, File.WRITE)
	file.store_var(Data)
	$Label.text = 'File Saved!'
	file.close()

func Load():
	if file.file_exists(filePath):
		file.open(filePath, File.READ)
		var string = ''
		for data in file.get_var():
			string += data+'\n'
		$Label.text = string
		string = ''
		file.close()
	else:
		$Label.text = 'File Not Found!'
