///Chicken Movement

{
    dir_last = dir;
    var j = 0;
    for(var i = 3; i > 0; i--)
        {
        available_way[3] = false;
        available_way[2] = false;
        available_way[1] = false;
        available_way[0] = false;
        }
    if !collision_point(x-10,y,obj_block,false,true)
        {
        available_way[0] = true;
        }
    if !collision_point(x+10,y,obj_block,false,true)
        {
        available_way[1] = true;
        }
    if !collision_point(x,y-10,obj_block,false,true)
        {
        available_way[2] = true;
        }
    if !collision_point(x,y+10,obj_block,false,true)
        {
        available_way[3] = true;
        }
    do
        {
        dir = irandom(3);
        while dir = dir_last dir = irandom(3);
        j++;
        if j = 5 then break;
        }
    until available_way[dir] = true;
    
    switch(dir)
        {
        case 0: //Left
            dir_x = -10;
            dir_y = 0;
            sprite_index = spr_chicken_left;
            with(follow_id) image_angle = 180; 
            break;
            
        case 1: //Right
            dir_x = +10;
            dir_y = 0;
            sprite_index = spr_chicken_right;
            with(follow_id) image_angle = 0; 
            break;
            
        case 2: //Up
            dir_x = 0;
            dir_y = -10;
            sprite_index = spr_chicken_up;
            with(follow_id) image_angle = 90; 
            break;
            
        case 3: //Down
            dir_x = 0;
            dir_y = +10;
            sprite_index = spr_chicken_down;
            with(follow_id) image_angle = 270; 
            break;
        }
    
    alarm[0] = irandom_range(30,60);
}
