/* eslint-disable no-unused-vars */
import { useState } from 'react';
import './App.css';

function App() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [message, setMessage] = useState('Waiting for action...');
	const apiUrl = 'http://localhost:8080';

	const handleRegister = async (e) => {
		e.preventDefault();
		try {
			const response = await fetch(`${apiUrl}/api/auth/register`, {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				body: JSON.stringify({ email, password }),
			});
			const data = await response.json();
			setMessage(`[Registration]: ${data.message || 'Error'}`);
		} catch (err) {
			setMessage('API connection error.');
		}
	};

	const handleLogin = async (e) => {
		e.preventDefault();
		try {
			const response = await fetch(`${apiUrl}/api/auth/login`, {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				credentials: 'include',
				body: JSON.stringify({ email, password }),
			});
			const data = await response.json();
			setMessage(
				`[Login]: ${response.ok ? data.message : 'Authentication failed.'}`,
			);
		} catch (err) {
			setMessage('API connection error.');
		}
	};

	const handleTestProtected = async () => {
		try {
			const response = await fetch(`${apiUrl}/api/protected`, {
				method: 'GET',
				credentials: 'include',
			});
			const data = await response.json();
			setMessage(
				`[Protected Area]: ${response.ok ? data.message : 'Access Denied (401 Unauthorized)'}`,
			);
		} catch (err) {
			setMessage('API connection error.');
		}
	};

	const handleLogout = async () => {
		try {
			const response = await fetch(`${apiUrl}/api/auth/logout`, {
				method: 'POST',
				credentials: 'include',
			});
			const data = await response.json();
			setMessage(`[Logout]: ${data.message}`);
		} catch (err) {
			setMessage('API connection error.');
		}
	};

	return (
		<div className='container'>
			<div className='header'>
				<h1>🔐 Secure Authentication Flow</h1>
			</div>

			<div className='cards-container'>
				{/* CARD 1: REGISTRATION */}
				<div className='card'>
					<h2>1. Registration (Hashing)</h2>
					<p>
						The backend receives your password and encrypts it using
						BCrypt before saving it to PostgreSQL. We never know
						your real password.
					</p>
					<form
						className='form-group'
						onSubmit={handleRegister}
					>
						<input
							type='email'
							placeholder='Your e-mail'
							onChange={(e) => setEmail(e.target.value)}
							required
						/>
						<input
							type='password'
							placeholder='Your password'
							onChange={(e) => setPassword(e.target.value)}
							required
						/>
						<button type='submit'>Register User</button>
					</form>
				</div>

				{/* CARD 2: LOGIN */}
				<div
					className='card'
					style={{ borderLeftColor: '#10b981' }}
				>
					<h2>2. Login (HttpOnly Cookie)</h2>
					<p>
						Upon validating credentials, the API injects a JWT
						directly into an HttpOnly Cookie in your browser,
						shielding against XSS attacks.
					</p>
					<form
						className='form-group'
						onSubmit={handleLogin}
					>
						<button
							type='submit'
							style={{
								backgroundColor: '#10b981',
								width: '100%',
							}}
						>
							Login (Using the e-mail and password above)
						</button>
					</form>
				</div>

				{/* CARD 3: PROTECTED AREA & LOGOUT */}
				<div
					className='card'
					style={{ borderLeftColor: '#8b5cf6' }}
				>
					<h2>3. Protected Area (Validation)</h2>
					<p>
						The browser sends the Cookie automatically. The API
						validates the JWT signature and releases the data.
					</p>
					<div className='form-group'>
						<button
							type='button'
							onClick={handleTestProtected}
							style={{ backgroundColor: '#8b5cf6', flex: 1 }}
						>
							Test Protected Endpoint
						</button>
						<button
							type='button'
							onClick={handleLogout}
							className='danger'
						>
							Logout (Delete Cookie)
						</button>
					</div>
				</div>
			</div>

			{/* STATUS BOX */}
			<div className='status-box'>📡 System Status: {message}</div>
		</div>
	);
}

export default App;
