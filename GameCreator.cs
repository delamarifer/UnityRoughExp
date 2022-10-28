using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor;

public class GameCreator : MonoBehaviour
{
    RecorderWindow recorderWindow;
    public Transform MotherSurfaces;
    public Camera cam;
    public GameObject grabber; 
    public GameObject grabberT; 

    public GameObject allgrabbers; 
    // public GameObject grabberTsub; 

    public GameObject slidecube;

    public Transform cube; 
    public Transform cubeT; 


    // public GameObject grabber; 
    // public GameObject grabberT;

    int video_frames = 35;
    
    float t;
    float t_waitup;
    float t2;
    float t3;
    float t4; 
    float t5; 
    Vector3 startPosition;
    Vector3 target;
    Vector3 gstartPosition;
    Vector3 gtarget;
    float timeToReachTarget = 1f;
    float waitdown = 0.2f;
    float waitup = 0.2f;
    float stoprec = 0.2f;
    float restartrec = 0.2f;
    float rightafterrec = 0.2f;
    bool moved_all = false;
    int countmoves = 0;
    bool resetted = false;

    Quaternion camogrot;
    

    bool goingup = true;
    float forward_by = 11f;

    int room_count = 0;
    List<float> rand_angles = new List<float>();

    List<float> rand_x = new List<float>();

    private RecorderWindow GetRecorderWindow()
    {
        return (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow));
    }
  
    
    Vector3 camog;
    // Start is called before the first frame update
    void Start()
    {
        recorderWindow = GetRecorderWindow();
        // rand_x.Add(-10.0f);
        rand_x.Add(-1.0f);
        rand_x.Add(-10.0f);
        // rand_x.Add(-0.02f);
        // cam.transform.LookAt(startPosition);
    // RandomAngles();
    //    RotateSurfaces();
    //    camogrot = cam.transform.rotation;
       camog = cam.transform.position;
       GetInitialPos();
       
        recorderWindow.StartRecording();
        
    }


    void RotateSurfaces()
    {
        int count = 0;
        foreach(Transform babysurf in MotherSurfaces){
            float rotatex = rand_angles[count];

            Vector3 newrot = babysurf.rotation.eulerAngles + new Vector3(0.0f, 0.0f, rotatex);
            babysurf.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, newrot, 1);
            count += 1;
        }
  
    }

    void RandomAngles()
    {
         //This is how you create a list. Notice how the type
        //is specified in the angle brackets (< >).
       

        for (int i = 0; i < 15; i++)
        {
            float rotatex = Random.Range(1.0f, 7.0f);
            rand_angles.Add(rotatex);
        }

     
    }

    void GetInitialPos()
    {
       


       
        camog = cam.transform.position;
        room_count += 1;
        
        forward_by = 11f - (0.5f*(room_count%2));
       
        Vector3 cube_prerot = cube.transform.position;
        // Debug.Log(cube_prerot);
        float rotatex = Random.Range(5.0f, 45.0f);
        Vector3 newrot = new Vector3(rotatex, 0.0f, 0.0f);
        slidecube.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, newrot, 1);
  
        startPosition = cube.transform.position;
        target = cubeT.transform.position;
        // Debug.Log(startPosition);
        Vector3 posdif = startPosition - target;
        grabber.transform.position = grabber.transform.position - (cube_prerot - startPosition);
        grabberT.transform.position = grabberT.transform.position - (cube_prerot - startPosition);
        grabberT.transform.position = grabberT.transform.position - posdif;

        gstartPosition = grabber.transform.position;
        gtarget = grabberT.transform.position;
        

     
        Vector3 cammove = new Vector3(rand_x[Random.Range(0, 2)], Random.Range(0.2f, 2.5f), Random.Range(0f, 0.72f));
        Debug.Log(cammove);
        cam.transform.position = cam.transform.position + cammove;
        cam.transform.LookAt(cubeT);

       
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(countmoves);
        // when 3500 frames pass:
        // reset angle (0, 270, 0)
        // move camera by -10 in the x direction
        if(countmoves < 2){
           
                if(t < timeToReachTarget){
                    // if(goingup)
                    // {
                        t += Time.deltaTime/timeToReachTarget;
                        // Debug.Log(t);
                        cube.transform.position = Vector3.Lerp(startPosition, target, t);
                        grabber.transform.position = Vector3.Lerp(gstartPosition, gtarget, t);

                    // }
                }
                else{

                    t2 += Time.deltaTime;
                    if(t2 >= waitdown){
                        countmoves += 1;
                        goingup = false;
                        t = 0;
                        t2 = 0;
                        t_waitup = 0;
                        Vector3 tempstartpos = startPosition;
                        startPosition = target;
                        target = tempstartpos;

                        Vector3 tempgstartpos = gstartPosition;
                        gstartPosition = gtarget;
                        gtarget = tempgstartpos;
                    }
                
            }
        }
        // if (recorderWindow.IsRecording())
                    

        if(countmoves > 1)
        {
            if(resetted == false){
                t3 += Time.deltaTime;
                if(t3 >= stoprec){

                    if (recorderWindow.IsRecording())
                        recorderWindow.StopRecording();
                
                    t4 += Time.deltaTime;
                    if(t4 >= restartrec){

                        // if(moved_all == false){
                            moved_all = true;
                            cam.transform.position = camog;
                            
                            cam.transform.rotation = Quaternion.Euler(0.0f,270f,0.0f);
                
                            cam.transform.Translate(Vector3.forward * forward_by); 
                            cube.transform.Translate(Vector3.forward * forward_by); 
                            // grabbersub.transform.Translate(Vector3.down * 20);
                            cubeT.transform.Translate(Vector3.forward * forward_by); 
                            // grabberT.transform.position = grabbersub.transform.position;
                            // grabberTsub.transform.Translate(Vector3.down * 20); 
                            grabberT.transform.position = gstartPosition;
                            allgrabbers.transform.Translate(Vector3.forward * forward_by); 
                            GetInitialPos();
                            t3 = 0;
                            t4 = 0;
                            resetted = true;
                            
                    }
                }
                
            }
            else{
                t_waitup += Time.deltaTime;
                if(t_waitup >= waitup){
                    if(!recorderWindow.IsRecording())
                        recorderWindow.StartRecording();
                    countmoves = 0;
                    resetted = false;
                    // // moved_all = true;
                    
                    // t5 = 0;
                    
                }

            }

            
        } 
        // Debug.Log(Time.frameCount);
        // Debug.Log(Time.deltaTime);
        
        
    }

    public void SetCubeDestination(Vector3 destination, float time)
     {
            t = 0;
            startPosition = transform.position;
            timeToReachTarget = time;
            target = destination; 
     }
}
