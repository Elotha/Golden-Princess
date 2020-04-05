///Walking Script

{
    if keyboard_check(ord('A')) && !keyboard_check(ord('D')) then h_dir = "Left";
    if !keyboard_check(ord('A')) && keyboard_check(ord('D')) then h_dir = "Right";
    if keyboard_check(ord('W')) && !keyboard_check(ord('S')) then v_dir = "Up";
    if !keyboard_check(ord('W')) && keyboard_check(ord('S')) then v_dir = "Down";
    
    if keyboard_check(ord('A')) && keyboard_check(ord('D')) then h_dir = "Stop";
    if !keyboard_check(ord('A')) && !keyboard_check(ord('D')) then h_dir = "Stop";
    if !keyboard_check(ord('W')) && !keyboard_check(ord('S')) then v_dir = "Stop";
    if keyboard_check(ord('W')) && keyboard_check(ord('S')) then v_dir = "Stop";
}
