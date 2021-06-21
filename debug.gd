extends Node


func _input(event) -> void:
	if event is InputEventKey and event.is_pressed():
		match event.scancode:
			KEY_F:
				OS.window_fullscreen = not OS.window_fullscreen
				OS.window_size = Vector2(1920, 1080)
			KEY_1:
				OS.window_size = Vector2(1920, 1080)
			KEY_2:
				OS.window_size = Vector2(1280, 720)
			KEY_3:
				OS.window_size = Vector2(854, 480)
			KEY_4:
				OS.window_size = Vector2(640, 360)
