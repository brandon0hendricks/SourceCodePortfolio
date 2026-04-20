package Cop2251.fall25.week1.hendricks;
//Student ID: 2438173
//Brandon Hendricks
public class Account {
	double balance;
	String accountNumber;
	double interestRate = .02; //not sure if this should be constant
	
	Account(String accountNumber,double balance){ //Constructor for account
		this.balance = balance;
		this.accountNumber = accountNumber;
	}
	public void deposit(double amount) { //adds to balance
		balance += amount;
	}
	public void withdraw(double amount) { //takes from balance
		if(amount <= balance) {
			balance-=amount; //subtract balance from the amount
		}
	}
	public double getBalance() { //returns balance value
		return balance;
	}
	public void addInterest() { //calculates and adds interest
		double interestToAdd = balance*interestRate; //calculates interest
		balance+=interestToAdd; //adds interest
	}

}
