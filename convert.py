import json, os, sys

input = json.loads(open("./Assets/Resources/country_codes.json").read())

output = dict(countries=[])

for i in input:
    output["countries"].append(dict(code=i, name=input[i]))

with open("./Assets/Resources/country_codes.json", "w") as f:
    f.write(json.dumps(output, indent=2))
