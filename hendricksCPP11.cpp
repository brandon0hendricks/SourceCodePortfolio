//Hendricks, Brandon
//March 2, 2026
//Chapter 7 Assignment
//Refrences
//Starting out with C++: from Control Structures to Objects (10th edition) by
//Tony Gaddis
//cplusplus.com

#include <iostream>
#include <iomanip>
#include <cstdlib>
#include <cstring>


#include <ctime>
using namespace std;

int main()
{
    //Step 1
    const int dArrSize = 4;
    double dArr[dArrSize]; //double array
    long lArr[7] = { 100000, //long array
        134567,
        123456,
        9,
        -234567,
        1,
        123489 };
    int iArr[3][5];//rows, columns
    char sName[30] = { 'B','r','a','n','d','o','n' };

    //Step 2
    short cnt1, cnt2; //variables for use
    long total;
    long highest;

    //Step 5 (initilized early)
    srand((unsigned int)time(NULL)); //random number generator starting point

    //Step 3
    for (int i = 0; i < dArrSize; i++) {
        //step 4
        dArr[i] = rand();
        //cout << dArr[i] << endl; //Testing purposes
    }
    //step 6
    for (int i = 0; i < dArrSize; i++) {     
        cout << dArr[i] << endl; //Testing purposes
    }
    //step 7
    total = 0;
    for (int i = 0; i < dArrSize; i++) {
        total += dArr[i];
    }
    //step 8
    cout << "Average: " << (total / dArrSize) << endl;
    //step 9
    for (cnt1 = 1, highest = lArr[0]; cnt1 < 7; cnt1++)
    {
        if (lArr[cnt1] > highest) {
            highest = lArr[cnt1];
        }
    }
    //step 10
    cout << "Highest: "<< highest << endl;
    //step 11
    for (cnt1 = 0; cnt1 < 3; cnt1++) {//rows
        for (cnt2 = 0; cnt2 < 5; cnt2++)//columns
        {
            iArr[cnt1][cnt2] = (rand() % 53) + 1;
        }
    }
    cout << endl;
    //step 12
    for (cnt1 = 0; cnt1 < 3; cnt1++) {//rows
        for (cnt2 = 0; cnt2 < 5; cnt2++)//columns
        {
            cout << setw(5) << iArr[cnt1][cnt2];
        }
        cout << endl;
    }
    cout << endl;
    //step 13
    for (cnt1 = 0; cnt1 < 5; cnt1++) {//columns
        for (cnt2 = 0; cnt2 < 3; cnt2++)//rows
        {
            cout << setw(5) << iArr[cnt2][cnt1];
        }
        cout << endl;
    }
    //step 14
    cout << "type your name: ";
    cin.getline(sName,30);
    //step 15
    cnt1 = 0;
    while (sName[cnt1] != '\0') {
        cout << setw(4) <<int(sName[cnt1]);
        cnt1++;
    }
    cout << endl;
    //step 16
    strcpy_s(sName, "Albert Einstein");
    cout << int(sName[11]);
    cout << endl << "Chapter 9:" << endl;
    //Chapter 9; step 1
    double *pdArray;
    //chapter 9: step 2
    pdArray = dArr;

    //Chapter 9: Step 3
    for (int i = 0; i < dArrSize; i++) {
        cout << dArr[i] << " ";
    }

    cout << endl;
    // Chapter 9: Step 4
    for (cnt1 = 0; cnt1 < dArrSize; cnt1++) {
        cout << *(pdArray + cnt1) << " ";
    }
    cout << endl;
    //chapter9: Step 5
    for (int i = 0; i < dArrSize; i++) {
        cout << *pdArray << " "; //print in pointer notation
        pdArray++;
    }
    cout << endl;
    //chapter 9: step 6
    for (cnt1 = 0; cnt1 < dArrSize; cnt1++) {
        cout << *(dArr + cnt1) << " ";
    }
    cout << endl;
    //chapter 9: Step 7
    int* piArray;
    piArray = new int[100];
    //chapter 10: step 8
    for (int i = 0; i < 100; i++){
        *(piArray + i) = (rand() % 49) + 1;
    }
    //chapter 10: Step 9
    for (int i = 0; i < 10; i++) {
        cout << *(piArray + i) << " ";
    }
    cout << endl;

    return 0;
}