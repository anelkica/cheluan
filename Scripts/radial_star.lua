local length = 10
local growth = 5
local thickness = 1

local star_color = "#FFDAA520"

turtle.color(star_color)

for i = 1, 60 do
    turtle.pen_size(thickness)
    turtle.move(length)
    
    turtle.turn(144.5) 
    
    length = length + growth
    --thickness = thickness + 0.1 -- Increases weight as it grows
end