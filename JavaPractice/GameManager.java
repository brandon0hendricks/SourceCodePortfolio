package application;

import javafx.scene.layout.BorderPane;
import javafx.scene.layout.StackPane;
import javafx.scene.paint.Color;
import javafx.scene.shape.Rectangle;
import javafx.scene.text.Font;
import javafx.scene.text.FontWeight;
import javafx.scene.text.Text;

public class GameManager extends BorderPane {
	int score = 0;
	Text scoreText = new Text(15,18,"Score: " + score); //sets score
	Paddle paddle;
	public GameManager(Paddle paddle) {
		scoreText.setFill(Color.BLACK);
		scoreText.setFont(Font.font("Arial", FontWeight.BOLD, 17));
		getChildren().add(scoreText);
		this.paddle = paddle;
	}
	
	void gameOver(BallPane ball) {
		ball.pause();
		
		Rectangle bkgShade = new Rectangle(getWidth(), getHeight());
		bkgShade.setFill(Color.rgb(0, 0, 0, 0.65)); // Black with 50% transparency
		//if I started earlier animation to drop down bkg would be added here.
		scoreText.setText(null); //clears score
		Text gameOver = new Text("GAME OVER"); //sets score
		gameOver.setFill(Color.WHITE);
		gameOver.setFont(Font.font("Arial", FontWeight.BOLD, 25));
		
		StackPane gameOverPane = new StackPane();
		gameOverPane.getChildren().addAll(bkgShade, gameOver);
		    
		setCenter(gameOverPane);
	}
	void updateScore() {
		 scoreText.setText("Score: " + score); 
	}

	int getScore() { 
		return score;
	}
	public void addScore() { //add this function to when the paddle collides
		score += 1;
		updateScore();
	}
}
