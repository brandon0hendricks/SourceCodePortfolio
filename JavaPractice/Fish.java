package Cop2251.fall25.week1.hendricks;

public abstract class Fish
	implements Comparable<Fish> {
	int zone; //top = 1, middle = 2, bottom = 3
	
	Fish(int zone){
		this.zone = zone;
	}
	public int getZone(){ //gets the fishes zone
		return zone;
	}
	public abstract String swim(); //how fish swims
	public abstract int getOxygenConsumption(); //oxygen needed by fish
	
	 @Override
	public int compareTo(Fish fish) { //interface function
		// TODO Auto-generated method stub
		return Integer.compare(this.zone, fish.getZone());
	}

}
