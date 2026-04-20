//Brandon Hendricks
//Febuary 23, 2026
//chapter 6 Assignment
//Refrences
//Starting out with C++: from Control Structures to Objects(10th edition) by
//cplusplus.com


#include <iostream>
#include <iomanip>

using namespace std;
//ProtoTyping
int findLowest(float score1,
	int score2,
	int score3,
	int score4,
	int score5);
void calcAverage(float score1,
	float score2,
	float score3,
	float score4,
	float score5);
void getScore(float &grade);


int main() {
	float score1, score2, score3, score4, score5;
	//this will get all scores.
	getScore(score1);
	getScore(score2);
	getScore(score3);
	getScore(score4);
	getScore(score5);

	calcAverage(score1, score2, score3, score4, score5);


}

void getScore(float &grade) { //Get score
	cout << "Input a test score 0 through 100: " << endl;
	cin >> grade;
	while ((grade < 0 && grade <100) ||(grade > 0 && grade >100))  { //Input validation
		cout << "Incorrect input: please input a value 0 through 100: " << endl;
		cin >> grade;
	}
}


void calcAverage(float score1, //pass five scores to get average
	float score2,
	float score3,
	float score4,
	float score5) {

	float sumOfGrades = (score1 + score2 + score3 + score4 + score5) //sum of all grades minus the lowest
		- findLowest(score1,
			score2,
			score3,
			score4,
			score5);
	float average = sumOfGrades / 4;
	cout << "The average amongst the scores is: " << average << endl;
	cout << "The dropped score was: " << findLowest(score1,
		score2,
		score3,
		score4,
		score5);
}
int findLowest(float score1, //gets lowest of five scores
	int score2,
	int score3,
	int score4,
	int score5) {


	int lowestValue = score1; //cycles though all values and assigns highest as it is seen
	
	if (lowestValue > score2) {
		lowestValue = score2;
	}
	if(lowestValue > score3){
		lowestValue = score3;
	}
	if (lowestValue > score4) {
		lowestValue = score4;
	}
	if (lowestValue > score5) {
		lowestValue = score5;
	}
	return lowestValue;
}


