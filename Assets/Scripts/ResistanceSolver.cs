using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ResistanceSolver
{
    private List<GameObject> line_database = new List<GameObject>();
    RaycastHit starting_point;
    RaycastHit ending_point;

    public ResistanceSolver(List<GameObject> all_lines) {
        line_database = all_lines;
        starting_point = all_lines[0].GetComponent<ConnectionInformation>().GetFirstHit();
        ending_point = all_lines[all_lines.Count - 1].GetComponent<ConnectionInformation>().GetSecondHit();
    }

    public float LaunchResolveResistance(){
        return ResolveResistance(line_database, starting_point, ending_point); //start the recursive process with the entire database. The starting and ending points are set as the battery and the ground. 
    }
    private float ResolveResistance(List<GameObject> this_segment, RaycastHit start, RaycastHit end) {
        
        float r_ges = 0;
        int cur_index = 0;
        //first create a simple straight path from the start to the end using the given information.
        List<GameObject> straight_path = FindStraightPathFromTo(start, end, this_segment);

        foreach (GameObject line in straight_path) {
            if (NumberOfBranchesStartingAt(line.GetComponent<ConnectionInformation>().GetFirstHit()) == 0) { //check if the head of the line has any branchings
                //if we are here it means that the line has no branchings and it can be added as usual.
                if (line == straight_path[straight_path.Count-1]) {
                    break; //remember not to consider the foot of the last line in the path since this may lead to false results when using this function in recursive calls. 
                }
                r_ges =+ line.GetComponent<ConnectionInformation>().GetSecondHit().collider.GetComponent<ElectricalComponent>().GetResistance();

            } else {
                //if we are here it means that we have encountered branchings at the head of the line
                //the first step is to find where these branchings end
                (GameObject outermost_end_of_branching, List<List<GameObject>> all_branches) = FindEndingsOfMultipleStraightPathsFromTo(line.GetComponent<ConnectionInformation>().GetFirstHit(), line_database.ToList<GameObject>(), straight_path);

                //now that we know where the branches end, call for each branch recursively the ResolveResistance
                foreach (List<GameObject> this_branch in all_branches) {
                    if (ListContainsThisLine(this_branch, line)) {
                        //remember to skip the lines that you are already considering in straight_path to prevent from calculating two times
                        continue;
                    }
                    r_ges = 1 / (1/r_ges + 1/ResolveResistance(straight_path.GetRange(cur_index, straight_path.Count-1), line.GetComponent<ConnectionInformation>().GetFirstHit(), outermost_end_of_branching.GetComponent<ConnectionInformation>().GetSecondHit()));
                }
            }
            cur_index++;
        }
        
        
        return r_ges;
    }

    private int NumberOfBranchesStartingAt(RaycastHit target) { //checks the heads of the lines if they are the same
        int n_of_branchings = 0;
        List<GameObject> branches = new List<GameObject>();
        
        foreach (GameObject line in line_database) {
            if (CompareEqual(line.GetComponent<ConnectionInformation>().GetFirstHit(), target)) {
                branches.Add(line);
                n_of_branchings++;
            }
        }

        return n_of_branchings-1;
    }

    private bool ListContainsThisLine(List<GameObject> my_list, GameObject my_line) {
        foreach (GameObject in_list in my_list) {
            if (CompareEqual(in_list.GetComponent<ConnectionInformation>().GetFirstHit(), my_line.GetComponent<ConnectionInformation>().GetFirstHit()) && CompareEqual(in_list.GetComponent<ConnectionInformation>().GetSecondHit(), my_line.GetComponent<ConnectionInformation>().GetSecondHit())) {
                return true;
            }
        }
        return false;
    }

    private int NumberOfBranchesEndingAt(RaycastHit target) { //checks the feet of the lines if they are the same
        int n_of_branchings = 0;
        List<GameObject> branches = new List<GameObject>();
        
        foreach (GameObject line in line_database) {
            if (CompareEqual(line.GetComponent<ConnectionInformation>().GetSecondHit(), target)) {
                branches.Add(line);
                n_of_branchings++;
            }
        }

        return n_of_branchings-1;
    }    

    private bool CompareEqual(RaycastHit obj1, RaycastHit obj2) {
        if (obj1.transform.GetInstanceID() == obj2.transform.GetInstanceID()) {
            return true;
        } else {
            return false;
        }
    }

    private List<GameObject> FindStraightPathFromTo(RaycastHit start, RaycastHit end, List<GameObject> database) {
        List<GameObject> computed_path = new List<GameObject>();
        bool allowexit = false;
        int original_size = database.Count;

        RaycastHit next = start;

        int n_out_iterations = 0;
        while (!allowexit && n_out_iterations <= original_size) {
            
            foreach (GameObject current_point in database.ToList<GameObject>()) {
                if (CompareEqual(current_point.GetComponent<ConnectionInformation>().GetFirstHit(),next)) {
                    computed_path.Add(current_point);
                    next = current_point.GetComponent<ConnectionInformation>().GetSecondHit();
                    if (CompareEqual(next,end)) { 
                        allowexit = true; //if we reach here it means that we have reached the ending point that we were looking for
                    }
                    database.Remove(current_point);

                    n_out_iterations++;
                    continue;
                }
            }
        Debug.Log("while1");
        }
        
        return computed_path;
    }

    private (GameObject, List<List<GameObject>>) FindEndingsOfMultipleStraightPathsFromTo(RaycastHit start_head, List<GameObject> database, List<GameObject> straight_path_to_outer_layer) {
        List<List<GameObject>> computed_paths = new List<List<GameObject>>();

        bool allowexit = false;
        bool allowouterexit = false;
        int original_size = database.Count;

        while (!allowouterexit) {
        Debug.Log("while2");
            RaycastHit next = start_head; //reset each time back to the start_head to start from where we started the first time.
            List<GameObject> single_path = new List<GameObject>();

            int innter_hit_counter = 0;
            while (!allowexit) {    
            Debug.Log("while3");
                

                foreach (GameObject current_point in database.ToList<GameObject>()) {
                    if (CompareEqual(current_point.GetComponent<ConnectionInformation>().GetFirstHit(),next)) {
                        single_path.Add(current_point);
                        next = current_point.GetComponent<ConnectionInformation>().GetSecondHit();
                        /*if (CompareEqual(next,end_foot)) { 
                            allowexit = true; //if we reach here it means that we have reached the ending point that we were looking for
                        }*/
                        database.Remove(current_point);

                        innter_hit_counter = 0;
                        continue;
                    }
                    innter_hit_counter++; //increment this if we cycled one time without finding anything
                }

                if (innter_hit_counter > 2) {
                    allowexit = true; //break the loop if we have cycled AT LEAST a couple of types the foreach loop without finding anything
                }
                
            }
            //if we reached here it means that we are done calculating a single path until we couldn't find any more connecting lines. Append the found path
            computed_paths.Add(single_path);
                
            if (database.Count == 0) { //only let this escape when we are done finding all other paths
                allowouterexit = true;
            }
            
        }
        
        //iterate the straight_path_to_outer_layer in reverse and check how many branchings we find. The first branching we find will be the end of all the branches
        foreach (GameObject line in straight_path_to_outer_layer.ToList<GameObject>()) {
            if (NumberOfBranchesEndingAt(line.GetComponent<ConnectionInformation>().GetSecondHit()) > 1) {
                return (line, computed_paths);
            }
        }

        Debug.LogError("oh shit! Something went wrong in ResistanceSolver::FindEndingsOfMultipleStraightPathsFromTo");
        return (null, null); //error!
    }
}
