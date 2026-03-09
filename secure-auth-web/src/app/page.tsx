import Link from 'next/link';

export default function Home() {
	return (
		<main className='flex min-h-screen flex-col items-center justify-center p-24 bg-linear-to-br from-gray-900 to-gray-800 text-white'>
			<div className='text-center'>
				<h1 className='text-5xl font-extrabold mb-6'>SecureAuth App</h1>
				<p className='text-xl text-gray-300 mb-10 max-w-2xl mx-auto'>
					An end-to-end authentication system utilizing Argon2, JWT, and HttpOnly Cookies.
				</p>

				<div className='flex gap-4 justify-center'>
					<Link
						href='/login'
						className='bg-blue-600 hover:bg-blue-500 text-white px-8 py-3 rounded-lg font-semibold transition'
					>
						Log In
					</Link>
					<Link
						href='/register'
						className='bg-gray-700 hover:bg-gray-600 text-white px-8 py-3 rounded-lg font-semibold transition'
					>
						Create Account
					</Link>
				</div>
			</div>
		</main>
	);
}
