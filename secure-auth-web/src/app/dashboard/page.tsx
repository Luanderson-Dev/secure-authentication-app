import { fetchProtectedData } from '@/lib/serverApi';
import LogoutButton from './LogoutButton';

export default async function DashboardPage() {
	const data = await fetchProtectedData();

	return (
		<main className='min-h-screen bg-gray-100 p-8 text-black'>
			<div className='max-w-4xl mx-auto bg-white p-8 rounded-lg shadow-sm border border-gray-200'>
				<header className='flex justify-between items-center mb-8 border-b pb-4'>
					<h1 className='text-3xl font-bold text-gray-800'>
						Secure Dashboard
					</h1>
					<LogoutButton />
				</header>

				<section className='bg-blue-50 border-l-4 border-blue-500 text-blue-800 p-6 rounded'>
					<h2 className='text-xl font-semibold mb-2'>
						Confidential Data
					</h2>
					<p className='mb-2'>
						<strong>API Message:</strong> {data.message}
					</p>
					<p>
						<strong>Authenticated User:</strong> {data.userEmail}
					</p>
				</section>
			</div>
		</main>
	);
}
