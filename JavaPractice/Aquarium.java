package Cop2251.fall25.week1.hendricks;

import java.util.*;

public class Aquarium{
	ArrayList<Fish> life = new ArrayList<>();
	int capacity; //oxygen content
	
	Aquarium(int width, int length){
		capacity = width*length;
	}

	public boolean add(Fish fish) {
		int currentOxygenUsage = 0;
		for(int i = 0; i < life.size(); i++) {
			currentOxygenUsage += life.get(i).getOxygenConsumption(); 
		}
		if(currentOxygenUsage + fish.getOxygenConsumption() <= capacity) {
			life.add(fish); //add fish if possible
			return true;
		}else {
			return false;
		}
	}
	public ArrayList<Fish> getFish(){ //getter for fish in tank
		return life;
	}
	public int getNumberOfFish() {
		return life.size(); //return amount of fish
	}
	public void empty() {
		life.clear(); //empty the tank
	}
	public void watch() { //compare fishes swim zone,
		for(int currentI = 0; currentI < life.size();currentI++) {
			for(int comparableI = currentI+1; comparableI < life.size();comparableI++) {
				if(life.get(currentI).compareTo(life.get(comparableI)) >= 0) { //if tested value is greater or equal to the one tested move it to its position
					Fish fishHolder = life.get(currentI);
					life.set(currentI, life.get(comparableI));
					life.set(comparableI, fishHolder); //switches position of two.		
				}
			}
			for(Fish fish : life) { //
				System.out.println(fish.swim()); //display fish swim pattern 
			}
		}
	}

	
}
