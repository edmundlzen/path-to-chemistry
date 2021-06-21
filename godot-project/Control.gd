extends Control

export (NodePath) var drop_path
onready var dropdown = get_node(drop_path)

# Array
var chemistry_array = ["Iron", "Magnesium", "Oksigen", "Clorin", "Carbon"]

func _ready():
	# Setup connection
	dropdown.connect("item_selected", self, "on_item_selected")
	
	# Add Items
	# add_items()
	
	# Add Cheemistry Items
	add_Chemistry_items()
	
	# Select item
	select_item(4)
	
	

	 
	
# Adding Items to Drop
func add_items():
	dropdown.add_item("Item 1")
	dropdown.add_separator()
	dropdown.add_item("Item 2")
	dropdown.add_separator()
	dropdown.add_item("Item 3")
	dropdown.add_separator()
	dropdown.add_item("Item 4")
	dropdown.add_separator()
	dropdown.add_item("Item 5")
	dropdown.add_separator()

# Adding Chemistry Items to Dropdown
func add_Chemistry_items():
	for item in chemistry_array:
		dropdown.add_item(item)

func select_item(id):
	dropdown.select(id)


func on_item_selected(id):
	# print(str(dropdown.get_item_text(id)))
	print(chemistry_array[id])
