using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public static class Values
    {
        public const int numTests = 6;
    }

    public static class Transitions
    {
        public static void run0to1()
        {

        }
        public static void run1to2()
        {

        }
        public static void run2to3()
        {
            GameObject.Find("u_i_1").GetComponent<u_i_1.CoroutineReplacement1>().enabled = true;
        }
        public static void run3to4()
        {
            GameObject.Find("u_i_1").GetComponent<u_i_1.CoroutineReplacement1>().enabled = false;
            GameObject.Find("u_i_2").GetComponent<u_i_2.CoroutineReplacement2>().enabled = true;
        }
        public static void run4to5()
        {
            GameObject.Find("u_i_2").GetComponent<u_i_2.CoroutineReplacement2>().enabled = false;
            GameObject.Find("u_i_3").GetComponent<u_i_3.CoroutineReplacement3>().enabled = true;
        }
        public static void run5to6()
        {
            GameObject.Find("u_i_3").GetComponent<u_i_3.CoroutineReplacement3>().enabled = false;
        }

        public static Action[] actions =
        {
            run0to1,
            run1to2,
            run2to3,
            run3to4,
            run4to5,
            run5to6
        };
    }
}

public class Benchmark : MonoBehaviour
{
    public GameObject mover;
    public GameObject resultUI;

    private switcher[] list;

    private float startTime;
    private int startCount;

    private int frameCount = 0;

    private float[] utimes = new float[Settings.Values.numTests];
    private float[] fps = new float[Settings.Values.numTests];
    private float[] dtimes = new float[Settings.Values.numTests];

    private float framesPerSecond;

    private bool autoMode = false;

    IEnumerator checker()
    {
        yield return 0;

        frameCount = 0;


        int j = 0;
        do
        {
            for (long i = 0; i < list.Length; i++)
            {
                list[i].switchToNextMethod();
            }

            Settings.Transitions.actions[j]();


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (long i = 0; i < list.Length; i++)
            {
                list[i].moveCurrentMethod();
            }

            stopwatch.Stop();
            utimes[j] = stopwatch.ElapsedMilliseconds;


            startCount = frameCount;
            startTime = Time.time;

            yield return new WaitForSeconds(30F);

            fps[j] = (frameCount - startCount) / (Time.time - startTime);


            stopwatch.Reset();
            stopwatch.Start();

            for (long i = 0; i < list.Length; i++)
            {
                list[i].stopCurrentMethod();
            }

            stopwatch.Stop();
            dtimes[j] = stopwatch.ElapsedMilliseconds;
        }
        while (++j < Settings.Values.numTests);


        startCount = frameCount;
        startTime = Time.time;

        yield return new WaitForSeconds(30F);

        framesPerSecond = (frameCount - startCount) / (Time.time - startTime);

        if(autoMode)
        {
            System.Console.WriteLine(
                ";no movement;" + list[0].getMover(0).getId() + ";" + list[0].getMover(1).getId() + ";" + list[0].getMover(5).getId() + ";" + list[0].getMover(2).getId() + ";" + list[0].getMover(3).getId() + ";" + list[0].getMover(4).getId() + "\n"
                + "prepare;-;" + Mathf.FloorToInt(utimes[0]) + ";" + Mathf.FloorToInt(utimes[1]) + ";" + Mathf.FloorToInt(utimes[5]) + ";" + Mathf.FloorToInt(utimes[2]) + ";" + Mathf.FloorToInt(utimes[3]) + ";" + Mathf.FloorToInt(utimes[4]) + "\n"
                + "run;" + Mathf.FloorToInt(framesPerSecond) + ";" + Mathf.FloorToInt(fps[0]) + ";" + Mathf.FloorToInt(fps[1]) + ";" + Mathf.FloorToInt(fps[5]) + ";" + Mathf.FloorToInt(fps[2]) + ";" + Mathf.FloorToInt(fps[3]) + ";" + Mathf.FloorToInt(fps[4]) + "\n"
                + "cleanup;-;" + Mathf.FloorToInt(dtimes[0]) + ";" + Mathf.FloorToInt(dtimes[1]) + ";" + Mathf.FloorToInt(dtimes[5]) + ";" + Mathf.FloorToInt(dtimes[2]) + ";" + Mathf.FloorToInt(dtimes[3]) + ";" + Mathf.FloorToInt(dtimes[4])
            );
            Application.Quit();
        }
        else
        {
            Instantiate(resultUI).GetComponentInChildren<Text>().text =
                  "no movement: " + Mathf.FloorToInt(framesPerSecond) + " fps\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[0]) + " ms\n"
                + list[0].getMover(0).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[0]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[0]) + " ms\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[1]) + " ms\n"
                + list[0].getMover(1).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[1]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[1]) + " ms\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[5]) + " ms\n"
                + list[0].getMover(5).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[5]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[5]) + " ms\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[2]) + " ms\n"
                + list[0].getMover(2).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[2]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[2]) + " ms\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[3]) + " ms\n"
                + list[0].getMover(3).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[3]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[3]) + " ms\n\n"
                + "    prepare: " + Mathf.FloorToInt(utimes[4]) + " ms\n"
                + list[0].getMover(4).getId() + ": <color=#ff0000ff>" + Mathf.FloorToInt(fps[4]) + "</color> fps\n"
                + "    cleanup: " + Mathf.FloorToInt(dtimes[4]) + " ms\n\n";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] args = Environment.GetCommandLineArgs();
        long numMovers = 1000;
        if(args.Length > 1)
        {
            try
            {
                long arg = Int64.Parse(args[1]);
                if(arg > 0)
                {
                    numMovers = arg;
                    autoMode = true;
                }
            }
            catch(Exception e)
            { }
        }
        list = new switcher[numMovers];
        for (long i =0; i < list.Length; i++)
        {
            list[i] = Instantiate(mover).GetComponent<switcher>();
        }
        StartCoroutine(checker());
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
    }
}
