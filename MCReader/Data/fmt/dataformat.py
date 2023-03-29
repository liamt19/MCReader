import json
import os
import pathlib

#   Generates tab-separated files for blocks, entities, and items.

thisPath = pathlib.Path(__file__).parent.resolve()
dataPath = os.path.join(thisPath, "BurgerData.json")
dataFile = open(dataPath, "r")
d = json.loads(dataFile.read())
dataFile.close()

blocks = d[0]["blocks"]["block"]
entities = d[0]["entities"]["entity"]
items = d[0]["items"]["item"]

blockFile = open(os.path.join(thisPath, "blocks.txt"), "w")

for block in blocks:
    d = blocks[block]
    st = ""
    if ("display_name" in d):
        st += d["display_name"] + "\t"
    else:
        st += "(unnamed)" + "\t"

    if ("numeric_id" in d):
        st += str(d["numeric_id"]) + "\t"
    else:
        st += "-1" + "\t"

    if ("text_id" in d):
       st += d["text_id"] + "\n"
    else:
        st += d["display_name"] + "\n"

    blockFile.write(st)

blockFile.close()



entityFile = open(os.path.join(thisPath, "entities.txt"), "w")

for entity in entities:
    d = entities[entity]
    st = ""
    if ("display_name" in d):
        st += d["display_name"] + "\t"
    else:
        st += "(unnamed)" + "\t"

    if ("id" in d):
        st += str(d["id"]) + "\t"
    else:
        st += "-1" + "\t"

    if ("name" in d):
       st += d["name"] + "\n"
    else:
        st += d["display_name"] + "\n"

    entityFile.write(st)
    
entityFile.close()


itemFile = open(os.path.join(thisPath, "items.txt"), "w")

for item in items:
    d = items[item]
    st = ""
    if ("display_name" in d):
        st += d["display_name"] + "\t"
    else:
        st += "(unnamed)" + "\t"

    if ("numeric_id" in d):
        st += str(d["numeric_id"]) + "\t"
    else:
        st += "-1" + "\t"

    if ("text_id" in d):
       st += d["text_id"] + "\n"
    else:
        st += d["display_name"] + "\n"

    itemFile.write(st)
    
itemFile.close()
