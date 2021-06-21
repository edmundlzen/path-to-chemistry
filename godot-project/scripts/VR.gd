extends ARVROrigin

func _ready():
	var VR_Interface = ARVRServer.find_interface("OpenVR")
	if (VR_Interface and VR_Interface.initialize()):
		get_viewport().arvr = true
		get_viewport().hdr = false
